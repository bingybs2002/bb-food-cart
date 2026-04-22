"use client"

import type { ColumnDef } from "@tanstack/react-table"

// This type is used to define the shape of our data.
// You can use a Zod schema here if you want.

export type OrderHistory = {
  id: number
  customerId: number
  isCheckedOut: boolean
  createdDate: string
  isCancelled: boolean
}

export const columns: ColumnDef<OrderHistory>[] = [
  {
    accessorKey: "id",
    header: "ID",
  },
  {
    accessorKey: "customerId",
    header: "Customer ID",
    filterFn: (row, columnId, filtervalue)=>{
      if(filtervalue==="") return true
      return row.getValue<number>(columnId) === Number(filtervalue)
    }
  },
  {
    accessorKey: "createdDate",
    header: "Created Date(UTC)",
  },
  {
    accessorKey: "isCheckedOut",
    header: "isCheckedOut",
  },
  {
    accessorKey: "isCancelled",
    header: "isCancelled",
  },
]