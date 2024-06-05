namespace API.Dtos.Systems
{
    public class TreeNode<T> where T : class
    {
        public string Label { get; set; }
        public T Data { get; set; }
        public string Icon { get; set; }
        public string ExpandedIcon { get; set; }
        public string CollapsedIcon { get; set; }
        public List<TreeNode<T>> Children { get; set; }
        public bool? Leaf { get; set; }
        public bool? Expanded { get; set; }
        public string Type { get; set; }
        public TreeNode<T> Parent { get; set; }
        public bool? PartialSelected { get; set; }
        public object Style { get; set; }
        public string StyleClass { get; set; }
        public bool? Draggable { get; set; }
        public bool? Droppable { get; set; }
        public bool? Selectable { get; set; }
        public string Key { get; set; }
    }
}