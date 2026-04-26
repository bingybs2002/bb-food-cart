import { columns, type OrderHistory } from "../OrderHistory/columns"
import { DataTable } from "../OrderHistory/data-table"
import { useEffect, useState } from "react"
import { AppSidebar } from "@/components/app-sidebar"
import { SidebarProvider } from "@/components/ui/sidebar"

export default function ShoppingCartTracker() {
  const [phoneNumber, setPhoneNumber] = useState(0)
  const [History, setHistory] = useState<OrderHistory[]>([])
  useEffect (()=>{
    fetch(`${import.meta.env.VITE_API_BASE_URL}/cartAdmin/{phoneNumber}`)
    .then((res)=>res.json())
    .then((data: OrderHistory[])=>setHistory(data))
  },[])
  return (
    <SidebarProvider>
        <AppSidebar/>
            <main className="relative w-full space-y-6 p-6">
                <h1 className="mt-4 text-center text-3xl font-bold lg:text-4xl">BB Food Cart | Shopping Cart Tracker</h1>
                <div className="container mx-auto py-10">
                <DataTable columns={columns} data={History} />
                </div>
            </main>
    </SidebarProvider>
  )
}
