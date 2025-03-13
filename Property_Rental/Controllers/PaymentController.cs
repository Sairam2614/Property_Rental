using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineRentalPropertyManagement.DTOs;
using OnlineRentalPropertyManagement.Models;
using OnlineRentalPropertyManagement.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "tenant")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);
                if (payment == null)
                {
                    return NotFound("Payment not found.");
                }

                var paymentDto = new PaymentDto
                {
                    PaymentID = payment.PaymentID,
                    LeaseID = payment.LeaseID,
                    Amount = payment.Amount,
                    PaymentDate = payment.PaymentDate,
                    Status = payment.Status.ToString()
                };

                return Ok(paymentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize(Roles = "tenant")]
        public async Task<IActionResult> GetAllPayments()
        {
            try
            {
                var payments = await _paymentService.GetAllPaymentsAsync();
                var paymentDtos = payments.Select(payment => new PaymentDto
                {
                    PaymentID = payment.PaymentID,
                    LeaseID = payment.LeaseID,
                    Amount = payment.Amount,
                    PaymentDate = payment.PaymentDate,
                    Status = payment.Status.ToString()
                });

                return Ok(paymentDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "tenant")]
        public async Task<IActionResult> AddPayment([FromBody] PaymentDto paymentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var isValid = await _paymentService.ValidatePaymentAmountAsync(paymentDto.LeaseID, paymentDto.Amount);
                if (!isValid)
                {
                    return BadRequest("Payment amount does not match the rent amount for the lease.");
                }

                var payment = new Payment
                {
                    LeaseID = paymentDto.LeaseID,
                    Amount = paymentDto.Amount,
                    PaymentDate = paymentDto.PaymentDate,
                    Status = Enum.Parse<Status>(paymentDto.Status)
                };

                await _paymentService.AddPaymentAsync(payment);
                return CreatedAtAction(nameof(GetPaymentById), new { id = payment.PaymentID }, paymentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "tenant")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] PaymentDto paymentDto)
        {
            try
            {
                if (id != paymentDto.PaymentID)
                {
                    return BadRequest("Payment ID mismatch.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var isValid = await _paymentService.ValidatePaymentAmountAsync(paymentDto.LeaseID, paymentDto.Amount);
                if (!isValid)
                {
                    return BadRequest("Payment amount does not match the rent amount for the lease.");
                }

                var payment = new Payment
                {
                    PaymentID = paymentDto.PaymentID,
                    LeaseID = paymentDto.LeaseID,
                    Amount = paymentDto.Amount,
                    PaymentDate = paymentDto.PaymentDate,
                    Status = Enum.Parse<Status>(paymentDto.Status)
                };

                await _paymentService.UpdatePaymentAsync(payment);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "tenant")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            try
            {
                await _paymentService.DeletePaymentAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}