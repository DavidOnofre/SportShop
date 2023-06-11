using Microsoft.AspNetCore.Mvc;
using SportShop.Models;
using SportsShop.Models;
using SportsShop.Repositorio.IRepositorio;
using System.Net;

namespace SportShop.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SportController : Controller
    {
        private readonly ILogger<SportController> _logger;
        private readonly ISportRepositorio _repositorio;
        protected APIResponse _response;

        //contructor
        public SportController(ILogger<SportController> logger, ISportRepositorio repositorio)
        {
            _logger = logger;
            _repositorio = repositorio;
            _response = new APIResponse();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAll()
        {
            try
            {
                _logger.LogInformation("Ingreso al metodo GetAll()");

                IEnumerable<Sport> sports = await _repositorio.GetAll();

                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.Result = sports;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error en el metodo GetAll()" + ex.Message);
                _response.isSuccess = false;
                _response.ErrorMessagge = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetById(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error con el id: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.isSuccess = false;
                    return BadRequest(_response);
                }

                var sport = await _repositorio.GetById(sport => sport.Id == id);
                if (sport == null)
                {
                    _logger.LogError("No se encontro el id: " + id);
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.isSuccess = false;
                    return NotFound(_response);
                }

                _response.Result = sport;
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErrorMessagge = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> Save([FromBody] Sport sport)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Error con los datos ingresados");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.isSuccess = false;
                    return BadRequest(_response);
                }

                if (sport == null)
                {
                    _logger.LogError("Entidad vacia");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.isSuccess = false;
                    return BadRequest(_response);
                }

                sport.CreatedDate = DateTime.Now;
                sport.UpdatedDate = DateTime.Now;

                await _repositorio.Save(sport);
                _response.Result = sport;
                _response.StatusCode = HttpStatusCode.Created;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErrorMessagge = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        // servicio rest
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var sport = await _repositorio.GetById(sport => sport.Id == id);
                if (sport == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _repositorio.Delete(sport);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErrorMessagge = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] Sport sport)
        {
            try
            {
                if (sport == null || id != sport.Id)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                await _repositorio.Update(sport);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErrorMessagge = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);
        }


        [HttpPost("SaveImage")]
        public async Task<String> SaveImage([FromForm] UploadImage uploadImage)
        {

            var route = String.Empty;

            if (uploadImage.file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + ".png";
                route = $"Images/{fileName}";

                using (var stream = new FileStream(route, FileMode.Create))
                {
                    await uploadImage.file.CopyToAsync(stream);
                }
            }

            return route;
        }

    }
}
