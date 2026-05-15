import { columns, type OrderHistory } from "./columns"
import { DataTable } from "./data-table"
import { useEffect, useState } from "react"
import { AppSidebar } from "@/components/app-sidebar"
import { SidebarProvider } from "@/components/ui/sidebar"
import { SidebarTrigger } from "@/components/ui/sidebar"
export default function OrderHistory() {

  const [History, setHistory] = useState<OrderHistory[]>([])

  useEffect (()=>{
    fetch(`${import.meta.env.VITE_API_BASE_URL}/cartAdmin/OrderHistory`)
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
            <SidebarTrigger className="absolute bottom-5 left-6 h-10 w-40 border-2 border-white" />
            </main>
    </SidebarProvider>
  )
}
