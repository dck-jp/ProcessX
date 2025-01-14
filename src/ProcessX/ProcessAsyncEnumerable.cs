﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Cysharp.Diagnostics
{
    public class ProcessAsyncEnumerable : IAsyncEnumerable<string>
    {
        readonly Process? process;
        readonly ChannelReader<string> channel;

        internal ProcessAsyncEnumerable(Process? process, ChannelReader<string> channel)
        {
            this.process = process;
            this.channel = channel;
        }

        public IAsyncEnumerator<string> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new ProcessAsyncEnumerator(process, channel, cancellationToken);
        }

        /// <summary>
        /// Consume all result and wait complete asynchronously.
        /// </summary>
        public async Task WaitAsync(CancellationToken cancellationToken = default)
        {
            await foreach (var _ in this.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
            }
        }

        /// <summary>
        /// Returning first value and wait complete asynchronously.
        /// </summary>
        public async Task<string> FirstAsync(CancellationToken cancellationToken = default)
        {
            string? data = null;
            await foreach (var item in this.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                data = (item ?? "");
            }

            if (data == null)
            {
                throw new InvalidOperationException("Process does not return any data.");
            }
            else
            {
                return data;
            }
        }

        /// <summary>
        /// Returning first value or null and wait complete asynchronously.
        /// </summary>
        public async Task<string?> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
        {
            string? data = null;
            await foreach (var item in this.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                data = (item ?? "");
            }
            return data;
        }

        public async Task<string[]> ToTask(CancellationToken cancellationToken = default)
        {
            var list = new List<string>();
            await foreach (var item in this.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                list.Add(item);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Write the all received data to console.
        /// </summary>
        public async Task WriteLineAllAsync(CancellationToken cancellationToken = default)
        {
            await foreach (var item in this.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                Console.WriteLine(item);
            }
        }
    }
}