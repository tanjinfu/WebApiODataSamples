using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.OData;
using Microsoft.OData.Edm;
using System.Web.OData.Extensions;
using System.Web.OData.Formatter;
using System.Web.OData.Builder;

namespace WebApplication1
{
    public class MyEnableQueryAttribute:EnableQueryAttribute
    {
        public override IEdmModel GetModel(Type elementClrType, HttpRequestMessage request,
            HttpActionDescriptor actionDescriptor)
        {
            // Get model for the request
            IEdmModel model = request.ODataProperties().Model;

            if (model == null)
            {
                // user has not configured anything or has registered a model without the element type
                // let's create one just for this type and cache it in the action descriptor
                model = actionDescriptor.Properties.GetOrAdd("System.Web.OData.Model+" + elementClrType.FullName, _ =>
                {
                    ODataConventionModelBuilder builder =
                        new ODataConventionModelBuilder(actionDescriptor.Configuration, isQueryCompositionMode: true);
                    builder.EnableLowerCamelCase();
                    EntityTypeConfiguration entityTypeConfiguration = builder.AddEntityType(elementClrType);
                    builder.AddEntitySet(elementClrType.Name, entityTypeConfiguration);
                    IEdmModel edmModel = builder.GetEdmModel();
                    Contract.Assert(edmModel != null);
                    return edmModel;
                }) as IEdmModel;
            }

            Contract.Assert(model != null);
            return model;
        }
    }
}