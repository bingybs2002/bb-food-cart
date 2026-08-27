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

  const foodTypeMap: Record<number, string> ={
      0: "Meal",
      1: "Beverage",
      2: "Side",
      3: "Combo",
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
    accessorKey: "nutrition",
    header: "Nutrition",
  },
  {
    accessorKey: "foodType",
    header: "Food Type",
    cell:({row}) => foodTypeMap[row.getValue("foodType") as number]
  },
  {
    accessorKey: "isSoldOut",
    header: "IsSOldOut",
  },
]