using Matc.Carpooling.Business;
using Matc.Carpooling.Entities;
using Matc.Carpooling.WebService.DataObjects;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Matc.Carpooling.WebService.Controllers
{
    public class RouteController : ApiController
    {
        #region Initialization
        readonly RouteBusiness routeBusiness = new RouteBusiness();
        readonly Validations validations = new Validations();
        #endregion

        /// <summary>
        /// Get all routes
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Routes/GetJourneyRoutes/")]
        public HttpResponseMessage GetJourneyRoutes(string journeyID)
        {
            if (!routeBusiness.IsJourneyIDValid(journeyID))
            {
                HttpError err = new HttpError("The journeyID you entered is not valid.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            return Request.CreateResponse(HttpStatusCode.OK, routeBusiness.GetJourneyRoutes(journeyID));
        }

        /// <summary>
        /// Get a route
        /// </summary>
        /// <param name="routeID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Routes/GetRoute/")]
        public HttpResponseMessage GetRoute(string routeID)
        {
            Route route = routeBusiness.GetRoute(routeID);
            if (route == null)
            {
                HttpError err = new HttpError("The routeID you entered is not valid.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            return Request.CreateResponse(HttpStatusCode.OK, route);
        }        

        /// <summary>
        /// Update price of a route
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/Routes/UpdateRoutePrice/")]        
        public HttpResponseMessage UpdateRoutePrice(RouteUpdateDto routeUpdateDto)
        {
            if(validations.IsNullOrEmpty(routeUpdateDto))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("Some values are missing"));
            }
            if (!routeBusiness.IsRouteIDValid(routeUpdateDto.RouteID))
            {
                HttpError err = new HttpError("The routeID you entered is not valid.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else if (!routeBusiness.IsPriceValid(routeUpdateDto.Price))
            {
                HttpError err = new HttpError("The price has to be higher than 0.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                routeBusiness.UpdateRoutePrice(routeUpdateDto.RouteID, routeUpdateDto.Price);
                return Request.CreateResponse(HttpStatusCode.OK, "Successfully updated the route price.");
            }
        }
    }
}
