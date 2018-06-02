using System.Collections.Generic;

namespace mdtemplate.Stories
{
    /// <summary>
    /// Defines a story.
    /// </summary>
    public class Story
    {
        /// <summary>
        /// An internal ID of the story. Not the workitem id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///  Determines the type of story.
        /// </summary>
        public StoryType StoryType { get; set; }

        /// <summary>
        /// Contents (HTML) of the description field.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Title of the story.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Contents (HTML) of the Acceptance Criteria field.
        /// </summary>
        public string AcceptanceCriteria { get; set; }

        /// <summary>
        /// All found properties in the YAML
        /// </summary>
        public Dictionary<string, string> Properties { get; } = new Dictionary<string, string>();

        /// <summary>
        /// List of hyperlinks
        /// </summary>
        public List<Hyperlink> Hyperlinks { get; set; } = new List<Hyperlink>();

        /// <summary>
        /// List of attachments
        /// </summary>
        public List<Hyperlink> Attachments { get; set; } = new List<Hyperlink>();

        /// <summary>
        /// Filename the contents came from.
        /// </summary>
        public string Filename { get; set; }
    }
}