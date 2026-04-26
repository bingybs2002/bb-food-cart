"use client"

import type { ColumnDef } from "@tanstack/react-table"

// This type is used to define the shape of our data.
// You can use a Zod schema here if you want.

export type Food = {
    id: number,
    name: string
}

export type ShoppingCartHistory = {
  id: number
  customerId: number
  foods: Food[]
  createdAtUtc: string
}

export const columns: ColumnDef<ShoppingCartHistory>[] = [
  {
    accessorKey: "id",
    header: "ID",
  },
  {
    accessorKey: "customerId",
    header: "Customer ID",
  },
  {
    accessorKey: "foods",
    header: "Food",
  },
  {
    accessorKey: "createdDate",
    header: "Created Date(UTC)",
  },
]