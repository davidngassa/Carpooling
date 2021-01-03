using Matc.Carpooling.Business;
using Matc.Carpooling.Entities;
using Matc.Carpooling.WebService.DataObjects;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace Matc.Carpooling.WebService.Controllers
{
    public class CheckpointController : ApiController
    {
        #region Initialization
        CheckpointBusiness checkpointBusiness = new CheckpointBusiness();
        JourneyBusiness journeyBusiness = new JourneyBusiness();
        RouteBusiness RouteBusiness = new RouteBusiness();
        Validations validations = new Validations();
        #endregion       
        
        /// <summary>
        /// Add new Checkpoint
        /// </summary>
        /// <param name="data"></param>l
        [HttpPost]
        [Route("api/Checkpoints/AddCheckpoint")]
        public HttpResponseMessage AddCheckpoint(CheckpointRegistrationDto checkpointRegistrationDto)
        {   
            if(validations.IsNullOrEmpty(checkpointRegistrationDto))
            {                
                return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("Some values are missing"));
            }
    
            float price = checkpointRegistrationDto.Price;
            string location = checkpointRegistrationDto.Location;
            string type = checkpointRegistrationDto.Type;
            string journeyId = checkpointRegistrationDto.JourneyId;

            bool IsCheckpointAdded = journeyBusiness.AddCheckpointToJourney(price, location, type, journeyId);

            if(!IsCheckpointAdded)
            {
                //gets error message
                var message = string.Format(journeyBusiness.CheckpointRegistrationValidation(price, location, type, journeyId));
                HttpError err = new HttpError(message);

                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                var message = string.Format("Checkpoint added successfully");
                RouteBusiness.AddRoutes(journeyId);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
        }

        /// <summary>
        /// Add a list of checkpoint
        /// </summary>
        /// <param name="data"></param>l
        [HttpPost]
        [Route("api/Checkpoints/AddCheckpointList")]
        public HttpResponseMessage AddCheckpointList(List<CheckpointRegistrationDto> checkpointRegistrationListDto)
        {
            bool IsCheckpointAdded;
            string message;

            foreach (CheckpointRegistrationDto Checkpoint in checkpointRegistrationListDto)
            {
                if (validations.IsNullOrEmpty(Checkpoint))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("Some values are missing"));
                }
            }

            foreach (CheckpointRegistrationDto Checkpoint in checkpointRegistrationListDto)
            {
                IsCheckpointAdded = journeyBusiness.AddCheckpointToJourney(Checkpoint.Price, Checkpoint.Location, Checkpoint.Type, Checkpoint.JourneyId);
                if (!IsCheckpointAdded)
                {
                    //gets error message
                    message = string.Format(journeyBusiness.CheckpointRegistrationValidation(Checkpoint.Price, Checkpoint.Location, Checkpoint.Type, Checkpoint.JourneyId));
                    HttpError err = new HttpError(message);

                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
            message = string.Format("Checkpoints added successfully");
            return Request.CreateResponse(HttpStatusCode.OK, message);
            
        }

        /// <summary>
        /// Get a checkpoint
        /// </summary>
        /// <param name="checkpointId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Checkpoints/GetCheckpoint")]
        public HttpResponseMessage GetCheckpoint(string journeyId, string checkpointId)
        {
            if (checkpointId == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("Please enter an ID for the checkpoint"));
            }

            Checkpoint checkpoint = checkpointBusiness.GetCheckpoint(journeyId, checkpointId);

            if(checkpoint == null)
            {
                var message = string.Format("Checkpoint not found");
                HttpError err = new HttpError(message);

                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }

            GetCheckpointDto getCheckpointDto = new GetCheckpointDto(checkpoint);

            return Request.CreateResponse(HttpStatusCode.OK, getCheckpointDto);            
        }

        /// <summary>
        /// Get all checkpoints for a journey
        /// </summary>
        /// <param name="journeyId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Checkpoints/GetJourneyCheckpoints")]
        public HttpResponseMessage GetJourneyCheckpoints(string journeyId)
        {
            if (journeyId == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("Please enter an ID for the journey"));
            }

            List<Checkpoint> checkpoints = checkpointBusiness.GetJourneyCheckpoints(journeyId);

            if(checkpoints.Count < 1)
            {
                var message = string.Format("No checkpoints have been created for this journey");
                HttpError err = new HttpError(message);

                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }

            //Iterates through the list of checkpoints and converts each of them to getCheckpoint DataObject
            List<GetCheckpointDto> getCheckpointDtos = new List<GetCheckpointDto>();

            foreach(Checkpoint checkpoint in checkpoints)
            {
                GetCheckpointDto getCheckpointDto = new GetCheckpointDto(checkpoint);
                getCheckpointDtos.Add(getCheckpointDto);
            }
            
            return Request.CreateResponse(HttpStatusCode.OK, getCheckpointDtos);            
        }

        /// <summary>
        /// Update Info about Checkpoint
        /// </summary>
        /// <param name="data"></param>
        [HttpPut]
        [Route("api/Checkpoints/UpdateCheckpoint")]
        public HttpResponseMessage UpdateCheckpointPrice(UpdateCheckpointDto updateCheckpointDto)
        {
            if(validations.IsNullOrEmpty(updateCheckpointDto))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("Some values are missing"));
            }          

            string checkpointId = updateCheckpointDto.Checkpoint_ID;
            string journeyId = updateCheckpointDto.Journey_ID;
            float price = updateCheckpointDto.Price;            

            bool commandSuccessful = checkpointBusiness.EditCheckpoint(journeyId, checkpointId, price);

            if(commandSuccessful == false)
            {
                var message = string.Format(checkpointBusiness.IsCheckpointEditable(journeyId, checkpointId));
                HttpError err = new HttpError(message);

                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                var message = string.Format($"Price updated successfully");

                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
        }

        /// <summary>
        /// Delete a checkpoint
        /// </summary>
        /// <param name="checkpointDeleteDto"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/Checkpoints/DeleteCheckpoint")]
        public HttpResponseMessage DeleteCheckpoint(string journeyId, string checkpointId)
        {
            var message = string.Empty;

            if (checkpointId == null || journeyId == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("Some parameters are missing"));
            }
            
            if(journeyBusiness.GetJourney(journeyId) == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Format("Journey not found"));
            }

            if(checkpointBusiness.GetCheckpoint(journeyId, checkpointId) == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Format("Checkpoint not found"));
            }

            bool isCheckpointDeleted = checkpointBusiness.DeleteCheckpoint(journeyId, checkpointId);

            if(isCheckpointDeleted == false)
            {
                message = string.Format("An error occured, action could not be completed");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }
            
            message = string.Format("Checkpoint deleted successfully");
            return Request.CreateResponse(HttpStatusCode.OK, message);            
        }
    }
}