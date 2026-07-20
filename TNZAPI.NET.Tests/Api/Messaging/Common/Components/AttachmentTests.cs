using TNZAPI.NET.Api.Messaging.Common.Components;

namespace TNZAPI.NET.Tests.Api.Messaging.Common.Components;

public class AttachmentTests
{
    [Fact]
    public void Constructor_WithExistingFileLocation_SetsFileNameLocationAndContent()
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, "hello world");

            var attachment = new Attachment(path);

            Assert.Equal(path, attachment.FileLocation);
            Assert.Equal(Path.GetFileName(path), attachment.FileName);
            Assert.NotNull(attachment.FileContent);
            Assert.NotEmpty(attachment.FileContent);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Constructor_WithNonExistentFileLocation_LeavesPropertiesAtDefaults()
    {
        var attachment = new Attachment(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));

        Assert.Equal("", attachment.FileLocation);
        Assert.Equal("", attachment.FileName);
        Assert.Equal("", attachment.FileContent);
    }

    [Fact]
    public void Constructor_WithFileNameAndContent_SetsFileNameAndContent()
    {
        var attachment = new Attachment("test.pdf", "base64content");

        Assert.Equal("test.pdf", attachment.FileName);
        Assert.Equal("base64content", attachment.FileContent);
    }

    [Fact]
    public void GetAttachment_WithExistingFile_ReturnsPopulatedAttachment()
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, "hello world");

            var attachment = Attachment.GetAttachment(path);

            Assert.NotNull(attachment);
            Assert.Equal(path, attachment.FileLocation);
            Assert.Equal(Path.GetFileName(path), attachment.FileName);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void GetAttachment_WithNonExistentFile_ReturnsNull()
    {
        var attachment = Attachment.GetAttachment(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));

        Assert.Null(attachment);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetAttachment_WithNullOrEmptyLocation_ReturnsNull(string? location)
    {
        var attachment = Attachment.GetAttachment(location);

        Assert.Null(attachment);
    }

    [Fact]
    public async Task GetAttachmentAsync_WithExistingFile_ReturnsPopulatedAttachment()
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, "hello world");

            var attachment = await Attachment.GetAttachmentAsync(path);

            Assert.NotNull(attachment);
            Assert.Equal(path, attachment.FileLocation);
            Assert.Equal(Path.GetFileName(path), attachment.FileName);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public async Task GetAttachmentAsync_WithNonExistentFile_ReturnsNull()
    {
        var attachment = await Attachment.GetAttachmentAsync(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));

        Assert.Null(attachment);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetAttachmentAsync_WithNullOrEmptyLocation_ReturnsNull(string? location)
    {
        var attachment = await Attachment.GetAttachmentAsync(location);

        Assert.Null(attachment);
    }

    [Fact]
    public void Dispose_ClearsFileNameContentAndLocation()
    {
        var attachment = new Attachment("test.pdf", "base64content");

        attachment.Dispose();

        Assert.Equal("", attachment.FileName);
        Assert.Equal("", attachment.FileContent);
        Assert.Equal("", attachment.FileLocation);
    }
}