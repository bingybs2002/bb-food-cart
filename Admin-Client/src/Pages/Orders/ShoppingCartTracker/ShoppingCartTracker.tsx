import { columns, type ShoppingCartHistory } from "../ShoppingCartTracker/columns"
import { DataTable } from "../ShoppingCartTracker/data-table"
import { useEffect, useState } from "react"
import { AppSidebar } from "@/components/app-sidebar"
import { SidebarProvider } from "@/components/ui/sidebar"
import { Card, CardHeader, CardTitle, CardDescription, CardFooter} from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"
import { SidebarTrigger } from "@/components/ui/sidebar"

const API = import.meta.env.VITE_API_BASE_URL

export default function ShoppingCartTracker() {

  const [History, setHistory] = useState<ShoppingCartHistory[]>([])
  const [phone, setPhone] = useState("")

  const res = async ()=>{
    try{
        const response = await fetch(`${API}/cartadmin/CustomerShoppingCartHistory/${phone}`, {
          method: "GET",
          headers: {
          Authorization: `Bearer ${localStorage.getItem("accessToken")}`
          }
        })
        if (!response.ok){
          throw new Error(`HTTP Error: ${response.status}`)
        }
        if(response){
          console.log("Not found")
        }
        const data: ShoppingCartHistory[] = await response.json()
        setHistory(data)
    }
    catch(error){
      console.error("Failed to fetch cart history: ", error)
      setHistory([])
    }
  }
  return (
    <SidebarProvider>
        <AppSidebar/>
            <main className="relative w-full space-y-6 p-6">
                <h1 className="mt-4 text-center text-3xl font-bold lg:text-4xl">BB Food Cart | Shopping Cart Tracker</h1>

                <div className="container mx-auto py-10">
                    <Card>
                        <CardHeader>
                            <CardTitle>Shopping Cart Tracker</CardTitle>  
                            <CardDescription>Search Cart by using Customer's Phone number</CardDescription>
                            <Input
                              placeholder="(---) --- -----"
                              value={phone}
                              onChange={(e)=>setPhone(e.target.value)}
                            />
                        </CardHeader> 
                        <CardFooter>
                            <Button
                                type="submit"
                                variant="outline" 
                                className="w-full"
                                onClick={res}
                            >Search</Button>
                        </CardFooter>
                    </Card>   
                 </div>
                <div className="container mx-auto py-10">
                <DataTable columns={columns} data={History} />
                </div>
                <SidebarTrigger className="absolute bottom-5 left-6 h-10 w-40 border-2 border-white" />
            </main>
    </SidebarProvider>
  )
}
