import { AppSidebar } from "@/components/app-sidebar";
import { SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar";
import {columns, type GachaItems } from "./columns";
import { DataTable } from "./data-table";
import { useEffect, useState } from "react";
import {
  Drawer,
  DrawerClose,
  DrawerContent,
  DrawerDescription,
  DrawerHeader,
  DrawerTitle,
  DrawerTrigger,
} from "@/components/ui/drawer"
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import {Toaster} from "@/components/ui/sonner"
import {toast} from "sonner"
const API = import.meta.env.VITE_API_BASE_URL


export default function GachaPricePool(){

  const [items, setItems] = useState<GachaItems[]>([])

  const [GachaPassword, setGachaPassword] = useState('')
  
  const [gachaPick, setGachaPick] = useState('')

  const [error, setError] = useState<string | null> (null)

  useEffect(()=>{
    fetch(`${API}/Gacha/all-items`)
    .then((res)=>res.json())
    .then((data: GachaItems[])=>setItems(data))
  }, [])

  const GetGachaRequest = async() => {
    try{
      setError(null)

      if(GachaPassword !== "bing"){
        toast.error("invalid admin password!",{
          description: "You are not allowed to draw!"
        })
        return
      }
      const res = await fetch(`${API}/Gacha/Testing-pick`)
      const data = await res.json()
      setGachaPick(data)
      console.log("Gacha result: ", data)
      console.log("Gacha Result: ", data.itemName)
      if(data.itemRarity===0){
        console.log("Fortune cookie.")
        toast.success("Fortune Coockie:", {
          description: data.itemName
        })
      }
      else{
      toast.success(data.itemName,{
        description: data.description
      })
      }
    } catch (err){
      console.error("Gacha request failed: ", err)
    }
  }

  return(
      <SidebarProvider>
          <AppSidebar/>
          <main className="relative w-full space-y-6 p-6">
            <h1 className="mt-4 text-center text-3xl font-bold lg:text-4xl">BB-Food | Price Pool</h1>
            <div className="container mx-auto py-10">
            </div>
            <h2>Gacha Items are listed below:</h2>
            <DataTable columns={columns} data={items} />

            <div className="flex justify-end border-white">
                <Drawer>
                  <Button className="text-2xl">
                      <DrawerTrigger>Open Drawing Menu</DrawerTrigger>
                  </Button>
                  <DrawerContent>
                    <DrawerHeader>
                      <DrawerTitle>
                        Gacha Draw!
                      </DrawerTitle>
                      <DrawerDescription>Admin only.</DrawerDescription>
                    </DrawerHeader>
                      <div className="mx-auto flex w-3/5 justify-center items-center gap-4">
                          <Input 
                            type="password" 
                            placeholder="Enter administrator password: "
                            value={GachaPassword}
                            onChange={(e)=>setGachaPassword(e.target.value)}
                            />
                        <DrawerClose asChild>
                          <Button 
                          onClick={
                            GetGachaRequest
                            }>DRAW!!!</Button>
                        </DrawerClose>


                      </div>
                      <div className="mt-10"></div>
                  </DrawerContent>
                </Drawer>
            </div>

          <SidebarTrigger className="absolute bottom-5 left-6 h-10 w-40 border-2 border-white" />
          <Toaster position="top-right" richColors/>
          </main>
      </SidebarProvider>
  )
}

