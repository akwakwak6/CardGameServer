namespace API.Models {
    public class SseModel {
        public string Id { get; set; }
        public string Type { get; set; }
        public IList<string> Data { get; set; }
    }

    public class TeamsSSE {
        public ICollection<Guid> teams { get; set; }
    }
}
