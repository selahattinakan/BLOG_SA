using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Elasticsearch.Models
{
    public class ES_Article
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("tags")]
        public string Tags { get; set; }

        [JsonPropertyName("created")]
        public string Created { get; set; } //Format must "dd.MM.yyyy HH:mm:ss"

        [JsonPropertyName("updated")]
        public string Updated { get; set; } //Format must "dd.MM.yyyy HH:mm:ss"
    }
}
