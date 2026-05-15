"use client"

import { type ColumnDef } from "@tanstack/react-table"

// This type is used to define the shape of our data.
// You can use a Zod schema here if you want.
export type GachaItems = {
  id: string
  itemName: string
  description: string
  itemRarity: number
}

export const columns: ColumnDef<GachaItems>[] = [
  {
   accessorKey: "id",
   header: "ID"
  },
  {
   accessorKey: "itemName",
   header: "Item Name"
  },
  {
   accessorKey: "description",
   header: "Description"
  },
  {
   accessorKey: "itemRarity",
   header: "Rarity"
  }
]