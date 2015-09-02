using StayWell.ServiceDefinitions.Content.Objects;
using StayWell.ServiceDefinitions.Mapping.Objects;
using StayWell.WebExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StayWell.WebExample.App_Code.Mappers
{
    public class ContentToGroupedContentModelMapper : IMapToNew<List<ContentResponse>,List<GroupedContentModel>>
    {
        public List<GroupedContentModel> Map(List<ContentResponse> source)
        {
            List<GroupedContentModel> groupedRelatedContent = new List<GroupedContentModel>();

            //Request the bucket to category configuration so that we know how to create the groups.
            ContentCategoryList categoryList = ContentCategoryFactory.GetRelatedServicesCategories();
            foreach (Category category in categoryList.Categories)
            {
                //Initilize the list the collection of content within the specific group.  Add
                //all content from "relatedContent" that match the bucket associated with the category.
                List<ContentResponse> groupedContent = new List<ContentResponse>();
                foreach (Bucket bucket in category.Buckets)
                {
                    groupedContent.AddRange(source.FindAll(c => c.Bucket.Slug == bucket.Slug));
                }

                if (groupedContent.Count > 0)
                {
                    List<ContentReferenceModel> contentList = new List<ContentReferenceModel>();
                    foreach (var item in groupedContent)
                    {
                        ContentReferenceModelMapper mapper = new ContentReferenceModelMapper();
                        contentList.Add(mapper.Map(item));
                    }

                    groupedRelatedContent.Add(new GroupedContentModel
                    {
                        Title = category.Names.Find(c => c.LanguageCode == "en").Value,
                        Slug = category.Slug,
                        Items = contentList
                    });
                }
            }

            return groupedRelatedContent;
        }
    }
}