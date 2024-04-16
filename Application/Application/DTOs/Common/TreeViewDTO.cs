namespace Application.DTOs
{
    /// <summary>
    /// نمایش بصورت درختی
    /// </summary>
    public class TreeViewDTO
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public long? ParentId { get; set; }
        public IList<TreeViewDTO> Nodes { get; set; }

    }
}
