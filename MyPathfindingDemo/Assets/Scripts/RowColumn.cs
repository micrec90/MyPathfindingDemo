﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public struct RowColumn
    {
        public int Row { get; }
        public int Column { get; }

        public RowColumn(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
