using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace MicroMMO
{
    public struct TileRef
    {
        public Tilemap Chunk;
        public Point ChunkCoordinates;
        public Point TileCoordinates;

        public override string ToString()
        {
            return String.Format("TileRef {{ Chunk: {0} ChunkCoordinates: {1} TileCoordinates: {2} }}",
                Chunk, ChunkCoordinates, TileCoordinates);
        }
    }
}
