
namespace QIQO.Business.ViewModels.Api
{
    public class EntityAttributeViewModel
    {
        //attributeKey : number;
        public int AttributeKey { get; set; }

        //entityKey : number;
        public int EntityKey { get; set; }

        //entityType : string;
        public string EntityType { get; set; }

        //attributeType : string;
        public string AttributeType { get; set; }

        //attributeValue : string;
        public string AttributeValue { get; set; }

        //attributeDataTypeKey : string;
        public string AttributeDataType { get; set; }

        //attributeDisplayFormat: string;
        public string AttributeDisplayFormat { get; set; }
    }
}
