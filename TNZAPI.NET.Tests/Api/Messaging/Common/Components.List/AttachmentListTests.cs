using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;

namespace TNZAPI.NET.Tests.Api.Messaging.Common.Components.List;

public class AttachmentListTests
{
    [Fact]
    public void ToList_CalledTwiceOnSameInstance_DoesNotDuplicateFileDerivedAttachments()
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, "hello world");

            using var list = new AttachmentList();
            list.Add(path);

            var first = list.ToList();
            var second = list.ToList();

            Assert.Single(first);
            Assert.Single(second);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public async Task ToListAsync_CalledTwiceOnSameInstance_DoesNotDuplicateFileDerivedAttachments()
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, "hello world");

            using var list = new AttachmentList();
            list.Add(path);

            var first = await list.ToListAsync();
            var second = await list.ToListAsync();

            Assert.Single(first);
            Assert.Single(second);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void ToList_CalledTwiceWithDirectlyAddedAttachment_DoesNotDuplicateIt()
    {
        using var list = new AttachmentList();
        list.Add(new Attachment("a.txt", "b"));

        var first = list.ToList();
        var second = list.ToList();

        Assert.Single(first);
        Assert.Single(second);
    }

    [Fact]
    public async Task ToListAsync_WithExistingFilePath_ReturnsPopulatedAttachment()
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, "hello world");

            using var list = new AttachmentList();
            list.Add(path);

            var result = await list.ToListAsync();

            Assert.NotNull(result);
            var attachment = Assert.Single(result);
            Assert.Equal(Path.GetFileName(path), attachment.FileName);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void ToList_WithExistingFilePath_ReturnsPopulatedAttachment()
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, "hello world");

            using var list = new AttachmentList();
            list.Add(path);

            var result = list.ToList();

            Assert.NotNull(result);
            var attachment = Assert.Single(result);
            Assert.Equal(Path.GetFileName(path), attachment.FileName);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void ToList_WithMissingFilePath_ThrowsFileNotFoundException()
    {
        var missingPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        using var list = new AttachmentList();
        list.Add(missingPath);

        var ex = Assert.Throws<FileNotFoundException>(() => list.ToList());
        Assert.Equal(missingPath, ex.FileName);
    }

    [Fact]
    public async Task ToListAsync_WithMissingFilePath_ThrowsFileNotFoundException()
    {
        var missingPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        using var list = new AttachmentList();
        list.Add(missingPath);

        var ex = await Assert.ThrowsAsync<FileNotFoundException>(() => list.ToListAsync());
        Assert.Equal(missingPath, ex.FileName);
    }

    [Fact]
    public void ToList_WithMissingFilePathMixedInWithValidOne_ThrowsFileNotFoundException()
    {
        var existingPath = Path.GetTempFileName();
        var missingPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        try
        {
            File.WriteAllText(existingPath, "hello world");

            using var list = new AttachmentList();
            list.Add(existingPath);
            list.Add(missingPath);

            Assert.Throws<FileNotFoundException>(() => list.ToList());
        }
        finally
        {
            File.Delete(existingPath);
        }
    }

    [Fact]
    public void Add_WithNullOrEmptyFilePath_IsIgnored()
    {
        using var list = new AttachmentList();

        list.Add((string?)null).Add("");

        var result = list.ToList();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void Add_WithAttachmentInstance_IncludesItInResult()
    {
        using var list = new AttachmentList();

        list.Add(new Attachment("test.pdf", "base64content"));

        var result = list.ToList();

        Assert.NotNull(result);
        var attachment = Assert.Single(result);
        Assert.Equal("test.pdf", attachment.FileName);
        Assert.Equal("base64content", attachment.FileContent);
    }

    [Fact]
    public void Add_WithNullAttachmentInstance_IsIgnored()
    {
        using var list = new AttachmentList();

        list.Add((Attachment)null!);

        var result = list.ToList();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void Add_WithAttachmentCollection_IncludesAllItemsInResult()
    {
        using var list = new AttachmentList();

        list.Add(new List<Attachment>
        {
            new Attachment("first.pdf", "content1"),
            new Attachment("second.pdf", "content2")
        });

        var result = list.ToList();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Add_WithNullOrEmptyAttachmentCollection_IsIgnored()
    {
        using var list = new AttachmentList();

        list.Add((ICollection<Attachment>?)null).Add(new List<Attachment>());

        var result = list.ToList();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void Add_WithStringCollection_IncludesAllExistingFilesInResult()
    {
        var path1 = Path.GetTempFileName();
        var path2 = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path1, "content1");
            File.WriteAllText(path2, "content2");

            using var list = new AttachmentList();
            list.Add(new List<string> { path1, path2 });

            var result = list.ToList();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
        finally
        {
            File.Delete(path1);
            File.Delete(path2);
        }
    }

    [Fact]
    public void ToList_WithNoFilesAdded_ReturnsEmptyCollection()
    {
        using var list = new AttachmentList();

        var result = list.ToList();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void Add_ReturnsSameInstanceForChaining()
    {
        using var list = new AttachmentList();

        var result = list.Add("file.txt").Add(new Attachment("a.txt", "b"));

        Assert.Same(list, result);
    }

    [Fact]
    public void ToList_AfterDispose_WithDirectlyAddedAttachment_ReturnsEmptyCollectionInsteadOfThrowing()
    {
        var list = new AttachmentList();
        list.Add(new Attachment("a.txt", "b"));
        list.Dispose();

        var result = list.ToList();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void ToList_AfterDispose_WithFilePathAdded_ReturnsEmptyCollectionInsteadOfReloadingFile()
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, "hello world");

            var list = new AttachmentList();
            list.Add(path);
            list.Dispose();

            var result = list.ToList();

            Assert.NotNull(result);
            Assert.Empty(result);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Add_AfterDispose_DoesNotThrow()
    {
        var list = new AttachmentList();
        list.Dispose();

        var result = list.Add(new Attachment("a.txt", "b"));

        Assert.Same(list, result);
        var attachment = Assert.Single(result.ToList());
        Assert.Equal("a.txt", attachment.FileName);
    }
}