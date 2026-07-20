import { useState, type FormEvent } from 'react'
import { Button } from '@/components/ui/button'
import { TextField, SelectField, type FieldValues } from '@/components/form-fields'
import { ResponseViewer } from '@/components/response-viewer'
import { apiRequest } from '@/lib/api-client'

// Mirrors tnzapi-ts-samples/nextjs/app/actions/reschedule (client.Actions.Reschedule.SendRequest),
// with the same fix as AbortPage: that reference's channel list is missing WhatsApp/RCS, but the
// .NET SDK's Actions interfaces (Core/Interfaces/Actions/*.cs) confirm Reschedule exists on all 7
// channels that have an Actions facade — SMS/Email/Fax/TTS/Voice/WhatsApp/RCS.
const CHANNELS = ['SMS', 'Email', 'Fax', 'TTS', 'Voice', 'WhatsApp', 'RCS']

export default function ReschedulePage() {
  const [fields, setFields] = useState<FieldValues>({ MessageID: '', Channel: 'SMS', SendTime: '' })
  const [status, setStatus] = useState<number | null>(null)
  const [data, setData] = useState<unknown>(null)
  const [submitting, setSubmitting] = useState(false)

  function setText(name: string, value: string) {
    setFields((f) => ({ ...f, [name]: value }))
  }

  async function handleSubmit(e: FormEvent) {
    e.preventDefault()
    setSubmitting(true)
    const body = {
      MessageID: (fields.MessageID as string) ?? '',
      Channel: (fields.Channel as string) ?? 'SMS',
      SendTime: (fields.SendTime as string) ?? '',
    }
    const { status: resStatus, data: resData } = await apiRequest('/api/actions/reschedule', { method: 'POST', body })
    setStatus(resStatus)
    setData(resData)
    setSubmitting(false)
  }

  return (
    <div className="max-w-2xl space-y-6">
      <div>
        <h1 className="text-xl font-semibold">Reschedule a Job</h1>
        <p className="text-sm text-muted-foreground">
          client.Actions.&lt;Channel&gt;.RescheduleAsync (supports SMS, Email, Fax, TTS, Voice, WhatsApp, RCS)
        </p>
      </div>
      <form onSubmit={handleSubmit} className="space-y-4">
        <TextField label="Message ID" name="MessageID" values={fields} onChange={setText} />
        <SelectField label="Channel" name="Channel" values={fields} onChange={setText} options={CHANNELS} />
        <TextField label="New Send Time" name="SendTime" values={fields} onChange={setText} placeholder="2026-08-01 09:00:00" />
        <Button type="submit" disabled={submitting}>
          {submitting ? 'Rescheduling...' : 'Reschedule'}
        </Button>
      </form>
      <ResponseViewer status={status} data={data} />
    </div>
  )
}