using Microsoft.AspNetCore.Mvc;
using Zadanie7.Models;

namespace Zadanie7.Controllers
{
    [ApiController]
    public class TripController : ControllerBase
    {
        private IRepositoryService _repositoryService = new RepositoryService();

        public TripController()
        {
        }


        [HttpGet]
        [Route("api/trips")]
        public ActionResult<IEnumerable<TripDTO>> Get()
        {
            return Ok(_repositoryService.GetTrips());
        }

        [HttpDelete]
        [Route("api/clients/{idClient}")]
        public ActionResult Delete(int idClient)
        {
            try
            {
                _repositoryService.DeleteClient(idClient);
                return Ok();
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/trips/{idTrip}/clients")]
        public ActionResult Post(int idTrip, ClientDTO client)
        {
            try
            {
                client.IdTrip = idTrip;
                _repositoryService.AddClientToTrip(client);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}