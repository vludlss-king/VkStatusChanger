namespace VkStatusChanger.Worker.Attributes
{
    internal class ChildVerbsAttribute : Attribute
    {
        public Type[] Types { get; }

        public ChildVerbsAttribute(params Type[] types)
        {
            Types = types;
        }
    }
}
