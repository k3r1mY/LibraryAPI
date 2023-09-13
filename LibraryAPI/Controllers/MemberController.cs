using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using LibraryAPI.dto;
using LibraryAPI.Services;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [Route("api/members")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly LibraryManagementSystemContext _context;
        private readonly MemberServices _memberServices;
        public MemberController(LibraryManagementSystemContext context, MemberServices memberServices)
        {
            _context = context;
            _memberServices = memberServices;
        }

        /// <summary>
        /// Returns all Members
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetLibraryMembers([FromQuery] int? id = null)
        {
            IQueryable<Member> query = _context.Members;
            if (id.HasValue)
            {
                Member memberDomain = await query.FirstOrDefaultAsync(memberObject => memberObject.MemberId == id.Value);

                if (memberDomain == null)
                {
                    return NotFound("No Member with that ID found.");
                }

                MemberDto memberDto = new MemberDto
                {
                    MemberId = memberDomain.MemberId,
                    FirstName = memberDomain.FirstName,
                    LastName = memberDomain.LastName,
                    PhoneNumber = memberDomain.PhoneNumber,
                    MembershipStatus = memberDomain.MembershipStatus,
                };
                return Ok(memberDto);
            }

            // If no id is provided, continue with displaying all members.
            List<Member> membersDomain = await query.ToListAsync();
            // Map domain to dto
            List<MemberDto> memberDtos = membersDomain.Select(membersDomain => new MemberDto
            {
                MemberId = membersDomain.MemberId,
                FirstName = membersDomain.FirstName,
                LastName = membersDomain.LastName,
                PhoneNumber = membersDomain.PhoneNumber,
                MembershipStatus = membersDomain.MembershipStatus,
            }).ToList();

            // Return DTO
            return Ok(memberDtos);
        }

        /// <summary>
        /// Add a new library member
        /// </summary>
        /// <param name="addMemberRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddLibraryMember([FromBody] AddMemberRequestDto addMemberRequest)
        {


            string salt;
            var hashedPassword = _memberServices.HashPassword(addMemberRequest.Password, out salt);

            Member newMember = new Member
            {
                FirstName = addMemberRequest.FirstName,
                LastName = addMemberRequest.LastName,
                PhoneNumber = addMemberRequest.PhoneNumber,
                MembershipStatus = "Active",
                Password = hashedPassword,
                Salt = salt,
            };
            await _context.Members.AddAsync(newMember);
            await _context.SaveChangesAsync();
            MemberDto memberDto = new MemberDto
            {
                MemberId = newMember.MemberId,
                FirstName = newMember.FirstName,
                LastName = newMember.LastName,
                PhoneNumber = newMember.PhoneNumber,
                MembershipStatus = newMember.MembershipStatus,
            };
            return CreatedAtAction(nameof(GetLibraryMembers), new { id = memberDto.MemberId }, memberDto);
        }

        /// <summary>
        /// Updates a library member using their id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="memberDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditLibraryMember(int id, [FromBody] AddMemberRequestDto memberDto)
        {

            string salt;
            var hashedPassword = _memberServices.HashPassword(memberDto.Password, out salt);
            Member existingMember = await _context.Members.FirstOrDefaultAsync(memberObject => memberObject.MemberId == id);

            if (existingMember == null)
            {
                return NotFound("Member not found.");
            }

            existingMember.FirstName = memberDto.FirstName;
            existingMember.LastName = memberDto.LastName;
            existingMember.PhoneNumber = memberDto.PhoneNumber;
            existingMember.MembershipStatus = memberDto.MembershipStatus;
            existingMember.Password = hashedPassword;
            existingMember.Salt = salt;

            await _context.SaveChangesAsync();

            // Map object to DTO
            MemberDto newMemberDto = new MemberDto
            {
                FirstName = memberDto.FirstName,
                LastName = memberDto.LastName,
                PhoneNumber = memberDto.PhoneNumber,
                MembershipStatus = memberDto.MembershipStatus,

            };

            return Ok(newMemberDto);
        }

        /// <summary>
        /// Delete a library member.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibraryMember(int id)
        {
            Member memberToDelete = _context.Members.FirstOrDefault(m => m.MemberId == id);
            if (memberToDelete == null)
            {
                return NotFound("Member not found.");
            }
            _context.Members.Remove(memberToDelete);
            _context.SaveChanges();
            return NoContent();
        }
    }
}


