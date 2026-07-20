// Never throws — a network failure resolves to status 0 with an { Error } payload instead of
// rejecting, so callers can always rely on `await apiRequest(...)` completing and don't each need
// their own try/catch just to reset a loading flag. A non-JSON response body still surfaces the
// real HTTP status (not 0) — only the fetch call itself failing (network down, CORS, etc.) is a
// true status-0 network error.
export async function apiRequest<T = unknown>(
  url: string,
  options: { method?: string; body?: unknown } = {},
): Promise<{ status: number; data: T }> {
  let res: Response
  try {
    res = await fetch(url, {
      method: options.method ?? 'GET',
      headers: options.body !== undefined ? { 'Content-Type': 'application/json' } : undefined,
      body: options.body !== undefined ? JSON.stringify(options.body) : undefined,
    })
  } catch (err) {
    const message = err instanceof Error ? err.message : 'Network error'
    return { status: 0, data: { Error: message } as T }
  }

  try {
    // res.text() first (not res.json() directly) so an empty body — 204/205, or any other response
    // with nothing to parse — is detected by content rather than inferred from status code or a
    // Content-Length header, which HTTP/2 and chunked-transfer responses aren't required to send.
    // Reading the body can itself throw (a dropped connection mid-stream), so this stays inside the
    // same try/catch res.json() used to be in — letting it escape uncaught would turn a bad response
    // into an unhandled rejection instead of the { status, data: { Error } } shape callers expect.
    const text = await res.text()
    if (!text) {
      // ResponseViewer treats any status < 400 as success (not res.ok's 200-299 — a 304 would count
      // as success there too), so a genuinely empty <400 body should fall back to `{}` — but an
      // empty 4xx/5xx body needs its own Error message, or it would render identically to a real
      // empty success with no indication anything went wrong. The `{}` fallback (not `null`) matches
      // the JSON-parse-failure branch below: every caller destructures `data` and reads a field off
      // it immediately (e.g. `data.MessageID`) without a null check, so `null as T` would just move
      // the crash from here to there.
      return res.status < 400
        ? { status: res.status, data: {} as T }
        : { status: res.status, data: { Error: `HTTP ${res.status} with an empty response body` } as T }
    }
    const data = JSON.parse(text) as T
    return { status: res.status, data }
  } catch {
    return { status: res.status, data: { Error: 'Response body was not valid JSON' } as T }
  }
}

// Attachment wire shape is identical across every messaging channel that supports Files —
// { FileName, FileContent } maps straight onto the SDK's Attachment(fileName, fileContent) ctor.
// PascalCase to match the C# request records verbatim (Program.cs sets PropertyNamingPolicy = null
// so the wire format matches the TNZ REST API's/SDK's own convention rather than JS camelCase).
export interface MessageAttachment {
  FileName: string
  FileContent: string
}

// Every field SMSModel supports, aside from ContactID/GroupID (addressbook references, need a
// contact/group picker rather than a text field — out of scope for this phase).
export interface SendSmsRequest {
  ToNumber: string
  Message: string
  Reference?: string
  TemplateId?: string
  NotificationType?: string
  WebhookCallbackUrl?: string
  WebhookCallbackFormat?: string
  ReportTo?: string
  SendTime?: string
  Timezone?: string
  SubAccount?: string
  Department?: string
  ChargeCode?: string
  FromNumber?: string
  SmsEmailReply?: string
  CharacterConversion?: boolean
  FallbackMode?: string[]
  SendMode?: string
  Attachments?: MessageAttachment[]
}

// Every field RCSModel supports, aside from ContactID/GroupID (addressbook references, need a
// contact/group picker rather than a text field — out of scope for this phase). Structurally
// identical to SendSmsRequest — only FallbackMode's accepted values differ (None/SMS/Voice/WhatsApp
// vs SMS's None/Voice/RCS/WhatsApp).
export interface SendRcsRequest {
  ToNumber: string
  Message: string
  Reference?: string
  TemplateId?: string
  NotificationType?: string
  WebhookCallbackUrl?: string
  WebhookCallbackFormat?: string
  ReportTo?: string
  SendTime?: string
  Timezone?: string
  SubAccount?: string
  Department?: string
  ChargeCode?: string
  FromNumber?: string
  SmsEmailReply?: string
  CharacterConversion?: boolean
  FallbackMode?: string[]
  SendMode?: string
  Attachments?: MessageAttachment[]
}

// Every field EmailModel supports, aside from ContactID/GroupID (addressbook references, need a
// contact/group picker rather than a text field — out of scope for this phase).
export interface SendEmailRequest {
  EmailAddress: string
  Subject: string
  MessageHtml: string
  Reference?: string
  TemplateId?: string
  SmtpFrom?: string
  From?: string
  FromEmail?: string
  ReplyTo?: string
  CcEmail?: string
  WebhookCallbackUrl?: string
  WebhookCallbackFormat?: string
  ReportTo?: string
  SendTime?: string
  Timezone?: string
  SubAccount?: string
  Department?: string
  ChargeCode?: string
  NotificationType?: string
  SendMode?: string
  Attachments?: MessageAttachment[]
}

// Every field FaxModel supports, aside from ContactID/GroupID (addressbook references, need a
// contact/group picker rather than a text field — out of scope for this phase). Fax has no
// message-text field of its own — the document to send is always an attachment.
export interface SendFaxRequest {
  ToNumber: string
  Attachments?: MessageAttachment[]
  Reference?: string
  TemplateId?: string
  NotificationType?: string
  WebhookCallbackUrl?: string
  WebhookCallbackFormat?: string
  ReportTo?: string
  SendTime?: string
  Timezone?: string
  SubAccount?: string
  Department?: string
  ChargeCode?: string
  Csid?: string
  Resolution?: string
  WatermarkFolder?: string
  WatermarkFirstPage?: string
  WatermarkAllPages?: string
  RetryAttempts?: number
  RetryPeriod?: number
  SendMode?: string
}

// Matches KeypadModel. PlayFile/File are Voice-only (file-based keypad playback) — TTS sends them
// as undefined, which is fine since the SDK treats them as no-ops for that channel.
export interface MessageKeypad {
  Tone: number
  Play?: string
  RouteNumber?: string
  PlaySection?: string
  PlayFile?: string
  File?: string
}

// Every field TTSModel supports, aside from ContactID/GroupID (addressbook references, need a
// contact/group picker rather than a text field — out of scope for this phase).
export interface SendTtsRequest {
  ToNumber: string
  MessageToPeople: string
  Reference?: string
  TemplateId?: string
  NotificationType?: string
  WebhookCallbackUrl?: string
  WebhookCallbackFormat?: string
  ReportTo?: string
  SendTime?: string
  Timezone?: string
  SubAccount?: string
  Department?: string
  ChargeCode?: string
  MessageToAnswerPhones?: string
  AnswerPhoneMode?: string
  Keypads?: MessageKeypad[]
  KeypadOptionRequired?: boolean
  CallRouteMessageOnWrongKey?: string
  CallRouteMessageToPeople?: string
  CallRouteMessageToOperators?: string
  EndCallMessage?: string
  NumberOfOperators?: number
  RetryAttempts?: number
  RetryPeriod?: number
  CallerId?: string
  Voice?: string
  Options?: string
  SendMode?: string
}

// Every field VoiceModel supports, aside from ContactID/GroupID (addressbook references, need a
// contact/group picker rather than a text field — out of scope for this phase) and VoiceFiles
// (undocumented in docs/voice.md — its relationship to Keypad.PlayFile isn't established anywhere
// in the SDK, so it was dropped from the UI as confusing rather than guessed at). Unlike TTS,
// there's no text-to-speech synthesis step — MessageToPeople/MessageToAnswerPhones/
// CallRouteMessage*/EndCallMessage are all base64 audio content (from SingleFileField), not literal
// spoken text, even though they're plain strings on the wire same as TTS's identically-named fields.
export interface SendVoiceRequest {
  ToNumber: string
  MessageToPeople?: string
  Reference?: string
  TemplateId?: string
  NotificationType?: string
  WebhookCallbackUrl?: string
  WebhookCallbackFormat?: string
  ReportTo?: string
  SendTime?: string
  Timezone?: string
  SubAccount?: string
  Department?: string
  ChargeCode?: string
  MessageToAnswerPhones?: string
  AnswerPhoneMode?: string
  Keypads?: MessageKeypad[]
  KeypadOptionRequired?: boolean
  CallRouteMessageOnWrongKey?: string
  CallRouteMessageToPeople?: string
  CallRouteMessageToOperators?: string
  EndCallMessage?: string
  NumberOfOperators?: number
  RetryAttempts?: number
  RetryPeriod?: number
  CallerId?: string
  Options?: string
  SendMode?: string
}

// Every field WhatsAppModel supports, aside from ContactID/GroupID (addressbook references, need a
// contact/group picker rather than a text field — out of scope for this phase). Custom1-9 are
// per-recipient personalisation values substituted into the approved template's
// [[Custom1]]..[[Custom9]] tokens (docs/whatsapp.md).
export interface SendWhatsAppRequest {
  ToNumber: string
  Message: string
  Attachments?: MessageAttachment[]
  Reference?: string
  TemplateId?: string
  FallbackMode?: string[]
  FromNumber?: string
  Custom1?: string
  Custom2?: string
  Custom3?: string
  Custom4?: string
  Custom5?: string
  Custom6?: string
  Custom7?: string
  Custom8?: string
  Custom9?: string
  NotificationType?: string
  WebhookCallbackUrl?: string
  WebhookCallbackFormat?: string
  ReportTo?: string
  SendTime?: string
  Timezone?: string
  SubAccount?: string
  Department?: string
  ChargeCode?: string
  SendMode?: string
}

// Every field WorkflowModel supports. Unlike every other channel, ContactIds/GroupIds are a
// first-class recipient mechanism here (not an out-of-scope addressbook picker) — comma-separated,
// split server-side. ToNumber/MainPhone/EmailAddress can all be set together for the SAME
// recipient (omni-channel — the Workflow Template decides which channel(s) actually get used).
// Workflow has no Message/TemplateID text content and no Status endpoint.
export interface SendWorkflowRequest {
  WorkflowTemplateId: string
  ToNumber?: string
  MainPhone?: string
  EmailAddress?: string
  ContactIds?: string
  GroupIds?: string
  Reference?: string
  NotificationType?: string
  WebhookCallbackUrl?: string
  WebhookCallbackFormat?: string
  SubAccount?: string
  Department?: string
  ChargeCode?: string
  SendTime?: string
  Timezone?: string
  SendMode?: string
}

// Every messaging channel's *ApiResult/*StatusApiResult share this same shape on the wire
// (Result/ErrorMessage/MessageID, plus JobStatus/Count/Complete/Success/Failed for status) — one
// pair of channel-agnostic types here instead of a near-identical interface per channel.
export interface MessageApiResult {
  Result: string
  ErrorMessage: string[]
  MessageID: string | null
}

export interface MessageStatusApiResult {
  Result: string
  ErrorMessage: string[]
  MessageID: string | null
  JobStatus: string
  Count: number
  Complete: number
  Success: number
  Failed: number
}