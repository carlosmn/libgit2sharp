using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibGit2Sharp
{
    [Flags]
    public enum MergeAnalysis
    {
        /// <summary>
        /// A "normal" merge; both HEAD and the given merge input have diverged
        /// from their common ancestor.  The divergent commits must be merged.
        /// </summary>
        Normal,

        /// <summary>
        /// All given merge inputs are reachable from HEAD, meaning the
        /// repository is up-to-date and no merge needs to be performed.
        /// </summary>
        UpToDate,

        /// <summary>
        /// The given merge input is a fast-forward from HEAD and no merge
        /// needs to be performed.  Instead, the client can check out the
        /// given merge input.
        /// </summary>
        FastForward,

        /// <summary>
        /// The HEAD of the current repository is "unborn" and does not point to
        /// a valid commit.  No merge can be performed, but the caller may wish
        /// to simply set HEAD to the target commit(s).
        /// </summary>
        Unborn,
    }
}
