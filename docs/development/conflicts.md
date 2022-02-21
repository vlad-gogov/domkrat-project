# Dealing with conflicts

Most merge conflicts for Unity meta files are being assumed to be resolved automatically with SmartMerge tool.

However, this possibility is not a certainty. Unity does a lot of work behind the scenes to create these files, and as such, changes that seem like they shouldn't conflict might end up conflicting. So, what should you do when they do conflict?

There are two options:

* _Manually merge the files together._ With this option, is it theoretically possible to keep both user's changes. However, especially with Unity generated text files, it may be extremely difficult or even impossible to understand what the various changes actually mean.
* _Choose one file to move forward with._ In the vast majority of merge conflicts with Unity, you'll likely want to move forward with just one item.


## Common conflict resolving flow

1. Run `git status` to see exactly what is conflicting. The conflicting files will show up in red, and git should tell you exactly what is wrong with them (_added by both_, _deleted by both_, _added by us_, etc.).
2. Use `git checkout --ours/--theirs path/to/the/file`. What this command will do is overwrite the file in question with either the file you are pulling in, or the file you are trying to push. The safest option is generally to use the `--theirs` option; hopefully, you know what changes you made. However, if you don't remember, or if the changes were of too large a scale, go ahead and talk to team and figure out what the conflicting changes actually are. Most of the time it's not a huge deal, in which case you're free to use the `--ours` option to overwrite their changes with yours. **Make sure to run this command for every conflicting file.**
3. Do `git add .` to add all changes detected in the repository to your commit.

Finally, you can commit and push the changes.


## Manual conflict resolving

We recommend to use Visual Studio Code for this. Open file with conflicts, then choose preferable changes for every conflicting block (with _Accept Incoming Change_, or _Accept Current Change_, or _Accept Both Changes_). Then add and commit these files as usual.


> Based on this article: https://uvasgd.github.io/sgd-docs/unity/github.html
