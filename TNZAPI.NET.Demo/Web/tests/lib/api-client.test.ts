import { describe, it, expect, vi, afterEach } from 'vitest'
import { apiRequest } from '@/lib/api-client'

describe('apiRequest', () => {
  afterEach(() => {
    vi.unstubAllGlobals()
  })

  it('defaults to a GET request with no body', async () => {
    const fetchMock = vi.fn().mockResolvedValue(new Response('{"Status":"ok"}', { status: 200 }))
    vi.stubGlobal('fetch', fetchMock)

    await apiRequest('/api/health')

    const [, init] = fetchMock.mock.calls[0]
    expect(init.method).toBe('GET')
    expect(init.body).toBeUndefined()
    expect(init.headers).toBeUndefined()
  })

  it('sends a JSON body and Content-Type header when a body is provided', async () => {
    const fetchMock = vi.fn().mockResolvedValue(new Response('{}', { status: 200 }))
    vi.stubGlobal('fetch', fetchMock)

    await apiRequest('/api/sms/send', { method: 'POST', body: { ToNumber: '+64211234567' } })

    const [, init] = fetchMock.mock.calls[0]
    expect(init.method).toBe('POST')
    expect(init.body).toBe('{"ToNumber":"+64211234567"}')
    expect(init.headers).toEqual({ 'Content-Type': 'application/json' })
  })

  it('returns the parsed body and status on success', async () => {
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue(new Response('{"MessageID":"abc-123"}', { status: 200 })))

    const result = await apiRequest<{ MessageID: string }>('/api/sms/status/abc-123')

    expect(result.status).toBe(200)
    expect(result.data.MessageID).toBe('abc-123')
  })

  it('never throws on a network failure — resolves to status 0 with an Error payload', async () => {
    vi.stubGlobal('fetch', vi.fn().mockRejectedValue(new TypeError('Failed to fetch')))

    const result = await apiRequest<{ Error: string }>('/api/sms/send', { method: 'POST', body: {} })

    expect(result.status).toBe(0)
    expect(result.data.Error).toBe('Failed to fetch')
  })

  it('never throws on a non-JSON response body — preserves the real HTTP status with an Error payload', async () => {
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue(new Response('not json', { status: 502 })))

    const result = await apiRequest<{ Error: string }>('/api/sms/send')

    expect(result.status).toBe(502)
    expect(result.data.Error).toBe('Response body was not valid JSON')
  })

  it('returns an empty object (not null) for a 204 No Content response, preserving the status', async () => {
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue(new Response(null, { status: 204 })))

    const result = await apiRequest<{ MessageID?: string }>('/api/addressbook/contacts/c-1', { method: 'DELETE' })

    expect(result.status).toBe(204)
    expect(result.data).toEqual({})
    // The fallback must not be `null` — every caller reads a field off `data` immediately after
    // destructuring, without a null check.
    expect(result.data.MessageID).toBeUndefined()
  })

  it('returns an empty object for a 200 with an empty body, regardless of Content-Length', async () => {
    // apiRequest detects "empty" via res.text() returning '', not via a Content-Length header — this
    // is the same path a real HTTP/2 or chunked-transfer empty response (neither required to send
    // Content-Length) would hit, so no header stubbing is needed to exercise it.
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue(new Response(null, { status: 200 })))

    const result = await apiRequest('/api/health')

    expect(result.status).toBe(200)
    expect(result.data).toEqual({})
  })

  it('returns an Error payload (not an empty success) for an error status with an empty body', async () => {
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue(new Response(null, { status: 502 })))

    const result = await apiRequest<{ Error: string }>('/api/sms/send')

    expect(result.status).toBe(502)
    expect(result.data.Error).toBe('HTTP 502 with an empty response body')
  })

  it('returns an empty object (not an Error) for an empty 3xx body — ResponseViewer treats <400 as success, not just 2xx', async () => {
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue(new Response(null, { status: 304 })))

    const result = await apiRequest('/api/health')

    expect(result.status).toBe(304)
    expect(result.data).toEqual({})
  })

  it('never throws when reading the response body itself fails', async () => {
    const res = new Response(null, { status: 200 })
    vi.spyOn(res, 'text').mockRejectedValue(new Error('stream aborted'))
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue(res))

    const result = await apiRequest<{ Error: string }>('/api/health')

    expect(result.status).toBe(200)
    expect(result.data.Error).toBe('Response body was not valid JSON')
  })
})