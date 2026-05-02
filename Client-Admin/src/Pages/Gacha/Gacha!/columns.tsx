"use client"

import type { ColumnDef } from "@tanstack/react-table"

// This type is used to define the shape of our data.
// You can use a Zod schema here if you want.



export type GachaItem = {
  id: number
  itemName: string
  description: string
  itemRarity: number
}

export const columns: ColumnDef<GachaItem>[] = [
  {
    accessorKey: "id",
    header: "ID",
  },
  {
    accessorKey: "itemName",
    header: "Name",
  },
  {
    accessorKey: "description",
    header: "Description",
  },
  {
    accessorKey: "itemRarity",
    header: "Rarity",
  },

]