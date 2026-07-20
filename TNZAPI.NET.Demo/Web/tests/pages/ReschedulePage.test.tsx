import { describe, it, expect, vi, afterEach } from 'vitest'
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import ReschedulePage from '@/pages/actions/ReschedulePage'
import { apiRequest } from '@/lib/api-client'

vi.mock('@/lib/api-client', async () => {
  const actual = await vi.importActual<typeof import('@/lib/api-client')>('@/lib/api-client')
  return { ...actual, apiRequest: vi.fn() }
})

const mockedApiRequest = vi.mocked(apiRequest)

describe('ReschedulePage', () => {
  afterEach(() => {
    mockedApiRequest.mockReset()
  })

  it('submits Message ID, Channel, and New Send Time', async () => {
    mockedApiRequest.mockResolvedValue({ status: 200, data: { ActionResult: 'OK' } })
    const user = userEvent.setup()
    render(<ReschedulePage />)

    await user.type(screen.getByLabelText('Message ID'), 'abc-123')
    await user.type(screen.getByLabelText('New Send Time'), '2026-08-01 09:00:00')
    await user.click(screen.getByRole('button', { name: 'Reschedule' }))

    expect(mockedApiRequest).toHaveBeenCalledWith(
      '/api/actions/reschedule',
      expect.objectContaining({
        method: 'POST',
        body: { MessageID: 'abc-123', Channel: 'SMS', SendTime: '2026-08-01 09:00:00' },
      }),
    )
  })

  it('offers all 7 channels, including WhatsApp and RCS', async () => {
    const user = userEvent.setup()
    render(<ReschedulePage />)

    await user.click(screen.getByLabelText('Channel'))

    for (const channel of ['SMS', 'Email', 'Fax', 'TTS', 'Voice', 'WhatsApp', 'RCS']) {
      expect(await screen.findByRole('option', { name: channel })).toBeInTheDocument()
    }
  })

  it('renders the response', async () => {
    mockedApiRequest.mockResolvedValue({ status: 200, data: { ActionResult: 'OK' } })
    const user = userEvent.setup()
    render(<ReschedulePage />)

    await user.type(screen.getByLabelText('Message ID'), 'abc-123')
    await user.type(screen.getByLabelText('New Send Time'), '2026-08-01 09:00:00')
    await user.click(screen.getByRole('button', { name: 'Reschedule' }))

    expect(await screen.findByText('Success (HTTP 200)')).toBeInTheDocument()
  })
})