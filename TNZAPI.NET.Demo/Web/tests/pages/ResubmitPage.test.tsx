import { describe, it, expect, vi, afterEach } from 'vitest'
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import ResubmitPage from '@/pages/actions/ResubmitPage'
import { apiRequest } from '@/lib/api-client'

vi.mock('@/lib/api-client', async () => {
  const actual = await vi.importActual<typeof import('@/lib/api-client')>('@/lib/api-client')
  return { ...actual, apiRequest: vi.fn() }
})

const mockedApiRequest = vi.mocked(apiRequest)

describe('ResubmitPage', () => {
  afterEach(() => {
    mockedApiRequest.mockReset()
  })

  it('submits Message ID and defaults Channel to Email', async () => {
    mockedApiRequest.mockResolvedValue({ status: 200, data: { ActionResult: 'OK' } })
    const user = userEvent.setup()
    render(<ResubmitPage />)

    await user.type(screen.getByLabelText('Message ID'), 'abc-123')
    await user.type(screen.getByLabelText('New Send Time'), '2026-08-01 09:00:00')
    await user.click(screen.getByRole('button', { name: 'Resubmit' }))

    expect(mockedApiRequest).toHaveBeenCalledWith(
      '/api/actions/resubmit',
      expect.objectContaining({
        method: 'POST',
        body: { MessageID: 'abc-123', Channel: 'Email', SendTime: '2026-08-01 09:00:00' },
      }),
    )
  })

  it('only offers Email/Fax/TTS/Voice — not SMS/WhatsApp/RCS', async () => {
    const user = userEvent.setup()
    render(<ResubmitPage />)

    await user.click(screen.getByLabelText('Channel'))

    for (const channel of ['Email', 'Fax', 'TTS', 'Voice']) {
      expect(await screen.findByRole('option', { name: channel })).toBeInTheDocument()
    }
    expect(screen.queryByRole('option', { name: 'SMS' })).not.toBeInTheDocument()
    expect(screen.queryByRole('option', { name: 'WhatsApp' })).not.toBeInTheDocument()
    expect(screen.queryByRole('option', { name: 'RCS' })).not.toBeInTheDocument()
  })

  it('renders the response', async () => {
    mockedApiRequest.mockResolvedValue({ status: 200, data: { ActionResult: 'OK' } })
    const user = userEvent.setup()
    render(<ResubmitPage />)

    await user.type(screen.getByLabelText('Message ID'), 'abc-123')
    await user.type(screen.getByLabelText('New Send Time'), '2026-08-01 09:00:00')
    await user.click(screen.getByRole('button', { name: 'Resubmit' }))

    expect(await screen.findByText('Success (HTTP 200)')).toBeInTheDocument()
  })
})