"use client"

import type { ColumnDef } from "@tanstack/react-table"

// This type is used to define the shape of our data.
// You can use a Zod schema here if you want.

export type MenuItems = {
  name: string
  description: string
  nutritionId: number
  nutrition: string
  allergies: number
  foodType: number
  isSoldOut: boolean
}

export const columns: ColumnDef<MenuItems>[] = [
  {
    accessorKey: "id",
    header: "ID",
  },
  {
    accessorKey: "name",
    header: "Name",
    
  },
  {
    accessorKey: "description",
    header: "Description",
  },
  {
    accessorKey: "nutrition",
    header: "Nutrition",
  },
  {
    accessorKey: "foodType",
    header: "Food Type",
  },
  {
    accessorKey: "isSoldOut",
    header: "IsSOldOut",
  },
]