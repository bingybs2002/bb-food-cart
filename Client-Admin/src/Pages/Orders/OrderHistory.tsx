import { columns, type OrderHistory } from "./OrderHistory/columns"
import { DataTable } from "./OrderHistory/data-table"
import React, { useEffect, useState } from "react"
import { AppSidebar } from "@/components/app-sidebar"
import { SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar"
import { Sidebar } from "lucide-react"
const data: OrderHistory[] = [
  {
    id: 1,
    customerId: 47,
    isCheckedOut: true,
    createdDate: "2026-04-16T10:37:18-07:00",
    isCancelled: false,
  },
  {
    id: 2,
    customerId: 41,
    isCheckedOut: true,
    createdDate: "2026-04-16T18:06:56-07:00",
    isCancelled: false,
  },
  {
    id: 3,
    customerId: 27,
    isCheckedOut: true,
    createdDate: "2026-04-16T16:02:48-07:00",
    isCancelled: false,
  },
  {
    id: 4,
    customerId: 28,
    isCheckedOut: true,
    createdDate: "2026-04-16T13:13:03-07:00",
    isCancelled: false,
  },
  {
    id: 5,
    customerId: 41,
    isCheckedOut: true,
    createdDate: "2026-04-16T21:15:25-07:00",
    isCancelled: false,
  },
]

export default function OrderHistory() {
  return (
    <SidebarProvider>
        <AppSidebar/>
            <main className="relative w-full space-y-6 p-6">
                <h1 className="mt-4 text-center text-3xl font-bold lg:text-4xl">BB Food Cart | Order History</h1>
                <div className="container mx-auto py-10">
                <DataTable columns={columns} data={data} />
                </div>
            </main>
    </SidebarProvider>



  )
}
