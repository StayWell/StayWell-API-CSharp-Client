using StayWell.ServiceDefinitions.Content.Objects;
using StayWell.ServiceDefinitions.Mapping.Objects;
using StayWell.WebExample.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace StayWell.WebExample.App_Code.Mappers
{
    public class MappingToGroupedContentModelMapper : IMapToNew<List<MappingResponse>, List<GroupedContentModel>>
    {
        public List<GroupedContentModel> Map(List<MappingResponse> source)
        {
            List<GroupedContentModel> groupedResponse = new List<GroupedContentModel>();
            var groupedMapping = source.GroupBy(c => c.MappingType);
            foreach (var group in groupedMapping)
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

                GroupedContentModel model = new GroupedContentModel();
                model.Title = "Client Mapping Demo: " + textInfo.ToTitleCase(group.Key);
                model.Slug = group.Key;

                //Add only a distinct set of mapped services.  It is possible to get multiple hits depending on how
                //the mapping was originally done.

                List<MappingResponse> subMappings = group.GroupBy(c => c.Name).Select(grp => grp.First()).ToList();
                List<ContentReferenceModel> referenceModels = new List<ContentReferenceModel>();
                foreach (var response in subMappings)
                {
                    referenceModels.Add(new ContentReferenceModel
                    {
                        Title = response.Name,
                        Uri = response.Uri,
                        Type = response.MappingType
                    });
                }
                model.Items = referenceModels;
                groupedResponse.Add(model);
            }

            return groupedResponse;
        }
    }
}