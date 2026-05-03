import { SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar"
import { AppSidebar } from "@/components/app-sidebar"

export default function GachaRulesPage() {
  return (
    <SidebarProvider>
      <AppSidebar />

      <main className="ml-10 flex-1 p-6 max-w-2xl border-1">
        <h2 className="scroll-m-20 border-b pb-2 text-3xl font-semibold tracking-tight first:mt-0">
          Gacha Rules and Disclaimers:
        </h2>
        <p className ="mt-4">
          Compared to traditional Chinese food Experience, we added Gacha on top of Fortune Cookies!
          For each purchase that is over $40, you get one spin!
        </p>

        <div>
          <h3 className="mt-20 text-lg font-semibold mb-2 mt-6">
            Drop Rate based on 30 rolls:
          </h3>

          <table className="mt-10 border-3 border-gray-300 text-sm">
              <tr>
                <th className="border px-3 py-2">Rarity</th>
                <th className="border px-3 py-2">Expected Rate</th>
                <th className="border px-3 py-2">Count (of 30)</th>
                <th className="border px-3 py-2">Actual Rate (%)</th>
              </tr>
            <tbody>
              <tr>
                <td className="border px-3 py-2">Common</td>
                <td className="border px-3 py-2">50%</td>
                <td className="border px-3 py-2">15</td>
                <td className="border px-3 py-2">50%</td>
              </tr>
              <tr>
                <td className="border px-3 py-2">Rare</td>
                <td className="border px-3 py-2">30%</td>
                <td className="border px-3 py-2">9</td>
                <td className="border px-3 py-2">30%</td>
              </tr>
              <tr>
                <td className="border px-3 py-2">Epic</td>
                <td className="border px-3 py-2">12%</td>
                <td className="border px-3 py-2">3</td>
                <td className="border px-3 py-2">10%</td>
              </tr>
              <tr>
                <td className="border px-3 py-2">Mystic</td>
                <td className="border px-3 py-2">7%</td>
                <td className="border px-3 py-2">2</td>
                <td className="border px-3 py-2">6.66%</td>
              </tr>
              <tr>
                <td className="border px-3 py-2">Legendary</td>
                <td className="border px-3 py-2">1%</td>
                <td className="border px-3 py-2">1</td>
                <td className="border px-3 py-2">3.33%</td>
              </tr>
            </tbody>
          </table>
          <h1 className="mt-10 scroll-m-20 text-center text-4xl font-extrabold tracking-tight text-balance">
              Once your order is completed, show it to me and I'll draw!
          </h1>
        </div>
        <SidebarTrigger className="absolute bottom-5 left-6 h-10 w-40 border-2 border-white" />
      </main>
    </SidebarProvider>
  )
}