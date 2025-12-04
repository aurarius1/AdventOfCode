#!/bin/bash

# Usage: ./newday.sh 02
DAY=$(printf "%02d" $1)  # Pad with zero
FOLDER="./$DAY"
CLASSNAME="Day$DAY"
NAMESPACE="_2025._$DAY"

# Create folders
mkdir "$FOLDER"
touch "$FOLDER/input"
touch "$FOLDER/example"

# Create C# class file
cat <<EOF > "$FOLDER/$CLASSNAME.cs"
using System.Diagnostics;
using System;

namespace $NAMESPACE
{
    public sealed class $CLASSNAME : Base
    {
        public $CLASSNAME(bool example) : base(example)
        {
            Day = "$DAY";
        }

        public override object PartOne()
        {
            return "";
        }

        public override object PartTwo()
        {
            return "";
        }
    }
}
EOF

echo "Created folder $FOLDER with class $CLASSNAME.cs"
