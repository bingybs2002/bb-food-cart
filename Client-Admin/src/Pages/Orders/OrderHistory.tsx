import { columns, type OrderHistory } from "./OrderHistory/columns"
import { DataTable } from "./OrderHistory/data-table"
import React, { useEffect, useState } from "react"
import { AppSidebar } from "@/components/app-sidebar"
import { SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar"
import { AppleIcon, Sidebar } from "lucide-react"
const API = import.meta.env.VITE_API_BASE_URL

export default function OrderHistory() {
  const [History, setHistory] = useState<OrderHistory[]>([])

  useEffect (()=>{
    fetch(`${API}/cartAdmin/OrderHistory`)
    .then((res)=>res.json())
    .then((data: OrderHistory[])=>setHistory(data))
  },[])
  return (
    <SidebarProvider>
        <AppSidebar/>
            <main className="relative w-full space-y-6 p-6">
                <h1 className="mt-4 text-center text-3xl font-bold lg:text-4xl">BB Food Cart | Order History</h1>
                <div className="container mx-auto py-10">
                <DataTable columns={columns} data={History} />
                </div>
            </main>
    </SidebarProvider>
  )
}
