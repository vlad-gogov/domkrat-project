# Setting up development environment

## Common prerequisities

* Unity 2021.1
* Visual Studio 2019
* Visual Studio Code
* Git


## Git LFS

We use Large Files Storage in this repository, so you need to have it installed over your git in order to work with huge binary files.

Please visit [official webiste](https://git-lfs.github.com/) for installation instructions and downloadable binaries. To make sure Git LFS was installed properly, run `git lfs` command in terminal.


## SmartMerge

To avoid conflicts with service files created by Unity, we use SmartMerge tool which comes right with Unity.

You just need to add these lines to the end of your `.git/config` file in this repository:

```conf
[merge "unityyamlmerge"]
	name = Unity SmartMerge (UnityYamlMerge)
	driver = \"/path/to/Unity/Editor/Data/Tools/UnityYAMLMerge.exe\" merge -h -p --force --fallback none %O %B %A %A
	recursive = binary
```

Don't forget to replace `/path/to/Unity` with real path to your Unity installation directory.
