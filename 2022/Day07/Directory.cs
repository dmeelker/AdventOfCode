using System.Diagnostics;

namespace AoC;

[DebuggerDisplay("{Name}")]
public class Directory
{
    public string Name { get; set; }
    public List<Directory> Directories { get; set; } = new();
    public long TotalFileSize { get; set; }

    public Directory(string name)
    {
        Name = name;
    }

    public long TotalDirectorySize => Directories.Sum(dir => dir.TotalSize);
    public long TotalSize => TotalDirectorySize + TotalFileSize;

    public IEnumerable<Directory> AllDirectories
    {
        get
        {
            yield return this;

            foreach (var subDirectory in Directories)
            {
                foreach (var subsub in subDirectory.AllDirectories)
                {
                    yield return subsub;
                }
            }
        }
    }
}
