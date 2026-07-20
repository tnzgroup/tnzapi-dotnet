using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Tests.Helpers;

public class FileHandlersTests
{
    [Fact]
    public void GetFileContents_WithNonExistentFile_ThrowsFileNotFoundException()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        Assert.Throws<FileNotFoundException>(() => FileHandlers.GetFileContents(path));
    }

    [Fact]
    public async Task GetFileContentsAsync_WithNonExistentFile_ThrowsFileNotFoundException()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        await Assert.ThrowsAsync<FileNotFoundException>(() => FileHandlers.GetFileContentsAsync(path));
    }

    [Fact]
    public void GetFileContentsGetBytes_WithNonExistentFile_ThrowsFileNotFoundException()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        Assert.Throws<FileNotFoundException>(() => FileHandlers.GetFileContentsGetBytes(path));
    }

    [Fact]
    public async Task GetFileContentsGetBytesAsync_WithNonExistentFile_ThrowsFileNotFoundException()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        await Assert.ThrowsAsync<FileNotFoundException>(() => FileHandlers.GetFileContentsGetBytesAsync(path));
    }

    [Fact]
    public void GetFileContents_WithExistingFile_ReturnsBase64Content()
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, "hello world");

            var result = FileHandlers.GetFileContents(path);

            Assert.Equal(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("hello world")), result);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public async Task GetFileContentsAsync_WithExistingFile_ReturnsBase64Content()
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, "hello world");

            var result = await FileHandlers.GetFileContentsAsync(path);

            Assert.Equal(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("hello world")), result);
        }
        finally
        {
            File.Delete(path);
        }
    }
}