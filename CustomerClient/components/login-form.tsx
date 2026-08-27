"use client"

import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import {
  Field,
  FieldDescription,
  FieldGroup,
  FieldLabel,
  FieldSeparator,
} from "@/components/ui/field"
import { Input } from "@/components/ui/input"
import { Eye, EyeOff, GalleryVerticalEndIcon } from "lucide-react"

import {useState} from "react"

export default function LoginForm({
  className,
  ...props
}: React.ComponentProps<"div">) {

const [showPassword, setShowPassword] = useState(false);

  return (
    <div className={cn("flex flex-col gap-6", className)} {...props}>
      <form>
        <FieldGroup>
          <div className="flex flex-col items-center gap-2 text-center">
            <a
              href="#"
              className="flex flex-col items-center gap-2 font-medium"
            >
              <div className="flex size-8 items-center justify-center rounded-md">
                <GalleryVerticalEndIcon className="size-6" />
              </div>
              <span className="sr-only">BB Food Cart</span>
            </a>
            <h1 className="text-xl font-bold">Welcome to BB Food Cart</h1>
          </div>
          <Field>
            <FieldLabel htmlFor="phone">Phone</FieldLabel>
            <Input
              id="phone"
              type="tel"
              placeholder="+1 (555) 123-4567"
              required
            />
          </Field>
          <Field>
            <FieldLabel htmlFor="password">Password</FieldLabel>
            <div className="relative">
              <Input
                id="password"
                type={showPassword ? "text" : "password"}
                placeholder="********"
                required
                className="pr-10"
              />
              <Button
                type="button"
                onClick={() => setShowPassword(!showPassword)}
                className="absolute right-2 top-1/2 -translate-y-1/2 p-1"
              >   
              {showPassword ? <EyeOff className="size-4" /> : <Eye className="size-4" />}
                </Button>
            </div>
          </Field>
          <Field>
            <Button type="submit">Login</Button>
          </Field>
            <FieldDescription>
              Don&apos;t have an account? <a href="SignUp">Sign up</a>
            </FieldDescription>
          <Field>

          </Field>
        </FieldGroup>
      </form>
    </div>
  )
}
