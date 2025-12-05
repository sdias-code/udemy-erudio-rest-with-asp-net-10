
using Microsoft.AspNetCore.Mvc;
using RestWithAspNet10_Scaffold.Services;
using RestWithAspNet10_Scaffold.Utils;


namespace RestWithAspNet10_Scaffold.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalcController : ControllerBase
    {
        private readonly NumberService _service;
        public CalcController(NumberService service)
        {
            _service = service;
        }

        [HttpGet("soma/{firstNumber}/{secondNumber}")]
        public IActionResult GetSum(string firstNumber, string secondNumber)
        {

            var result = _service.Sum(firstNumber, secondNumber);

            if (result.HasValue)
                return Ok(result.Value);

            return BadRequest("Invalid Input");

        }

        [HttpGet("subtracao/{firstNumber}/{secondNumber}")]
        public IActionResult GetSub( string firstNumber, string secondNumber)
        {
            var result = _service.Subtract(firstNumber, secondNumber);

            if (result.HasValue)
                return Ok(result.Value);

            return BadRequest("Invalid Input");
        }

        [HttpGet("divisao/{firstNumber}/{secondNumber}")]
        public IActionResult GetDiv(string firstNumber, string secondNumber)
        {
            var result = _service.Divide(firstNumber, secondNumber);

            if (!result.HasValue)
                return BadRequest("Invalid Input or division by zero");

            return Ok(result.Value);
        }

        [HttpGet("multiplicacao/{firstNumber}/{secondNumber}")]
        public IActionResult GetMult(string firstNumber, string secondNumber)
        {
            var result = _service.Multiply(firstNumber, secondNumber);

            if (result.HasValue)
                return Ok(result.Value);

            return BadRequest("Invalid Input");
        }

        [HttpGet("media/{firstNumber}/{secondNumber}")]
        public IActionResult GetMedia(string firstNumber, string secondNumber)
        {
            var result = _service.Mean(firstNumber, secondNumber);

            if (result.HasValue)
                return Ok(result.Value);

            return BadRequest("Invalid Input");
        }

        [HttpGet("raiz/{number}")]
        public IActionResult GetSquareRoot(string number)
        {
            try
            {
                var result = _service.SquareRoot(number);

                if (result.HasValue)
                    return Ok(result.Value);

                return BadRequest("Invalid Input");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
