using Microsoft.AspNetCore.Mvc;

namespace TestJrAPI.Controllers {
    [Route("/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase {

        [HttpPost]
        public async Task<IActionResult> Create(){
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int rows = 10) {

            if(rows > 30) {
                return BadRequest("The number of rows cannot exceed 10");
            }

            return Ok();

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {

            return Ok();

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id) {

            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) {

            return Ok();

        }



    }
}
