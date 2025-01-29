using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Api.Dtos;
using OrderManagement.Api.Errors;
using OrderManagement.Core;
using OrderManagement.Core.Entities;

namespace OrderManagement.Api.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class InvoicesController : APIBaseController
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public InvoicesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<InvoiceDto>>> GetAllInvoices()
        {
            var Invoices = await _unitOfWork.Repository<Inovice>().GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<InvoiceDto>>(Invoices));

        }
        [HttpGet("{InvoiceId}")]
        public async Task<ActionResult<InvoiceDto>> GetById(int InvoiceId)
        {
            var Invoices = await _unitOfWork.Repository<Inovice>().GetByIdAsync(InvoiceId);
            if (Invoices == null) return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<InvoiceDto>(Invoices));
        }
    }
}
