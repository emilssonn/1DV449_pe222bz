﻿using FindMyHome.Domain.Abstract;
using FindMyHome.Domain.Entities.Foursquare;
using FindMyHome.Domain.Exceptions;
using FindMyHome.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FindMyHome.Controllers
{
	//[ValidateHttpAntiForgeryTokenAttribute]
    public class CategoriesController : ApiController
    {
        private IFindMyHomeService _service;

        public CategoriesController(IFindMyHomeService service)
        {
            this._service = service;
        }

        // GET api/categories
        public IEnumerable<Category> Get()
        {
            try
            {
                return this._service.RefreshCategories();
            }
            catch (ExternalDataSourceException e)
            {
                HttpError err = new HttpError(e.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }
            catch (Exception e)
            {
                var message = string.Format(Properties.Resources.InternalServerError);
                HttpError err = new HttpError(message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }    
        }

        // PUT api/categories/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/categories/5
        public void Delete(int id)
        {
        }

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            this._service.Dispose();
            base.Dispose(disposing);
        }

        #endregion
    }
}
