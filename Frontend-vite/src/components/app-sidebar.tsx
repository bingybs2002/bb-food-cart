import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarGroup,
  SidebarGroupAction,
  SidebarGroupContent,
  SidebarGroupLabel,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarTrigger,
} from "@/components/ui/sidebar"
import {Link} from "react-router-dom"
import {
    DropdownMenu, 
    DropdownMenuTrigger, 
    DropdownMenuContent,
    DropdownMenuItem,
} from "@/components/ui/dropdown-menu"
import { ChevronDown, Divide, Plus, User2 } from "lucide-react"
import logo from "@/assets/bb food cart.png" 
import { Separator } from "./ui/separator"

export function AppSidebar() {
  return (
    <Sidebar>
{/*Header*/}
        <SidebarHeader>
            <div className="flex justify-center">
                <img src={logo} alt="bb food cart.png" className=" w-30 h-auto " />
                <a href="#" className="flex items-center text-2xl">
                BB Food Cart
                </a>
            </div>
            <Separator/>
        </SidebarHeader>

{/*Content*/}
        <SidebarContent className="py-4">
        <SidebarGroup className="space-y-5">

            <div>
            <SidebarGroupLabel className="ml-2 text-sm font-semibold">
                Orders
            </SidebarGroupLabel>
            <SidebarGroupContent className="ml-4">
                <SidebarMenu className="space-y-1">
                <SidebarMenuItem><Link to="">New Order</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="">Shopping Cart</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="">Order History</Link></SidebarMenuItem>
                </SidebarMenu>
            </SidebarGroupContent>
            </div>

            <div>
            <SidebarGroupLabel className="ml-2 text-sm font-semibold">
                Fitness Journey
            </SidebarGroupLabel>
            <SidebarGroupContent className="ml-4">
                <SidebarMenu className="space-y-1">
                <SidebarMenuItem><Link to="">Weigh In</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="">History</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="">Tips</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="">Tools</Link></SidebarMenuItem>
                </SidebarMenu>
            </SidebarGroupContent>
            </div>

            <div>
            <SidebarGroupLabel className="ml-2 text-sm font-semibold">
                Gacha
            </SidebarGroupLabel>
            <SidebarGroupContent className="ml-4">
                <SidebarMenu className="space-y-1">
                <SidebarMenuItem><Link to="">Try Your Luck</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="">Prize Pool</Link></SidebarMenuItem>
                <SidebarMenuItem><Link to="">Rules</Link></SidebarMenuItem>
                </SidebarMenu>
            </SidebarGroupContent>
            </div>

        </SidebarGroup>
        </SidebarContent>

{/*Footer*/}
      <SidebarFooter>
        <SidebarMenu>
          <SidebarMenuItem>
            <SidebarMenuButton>
              <User2 />
              Username
            </SidebarMenuButton>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarFooter>
    </Sidebar>
  )
}