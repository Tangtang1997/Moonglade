﻿using Moonglade.Data;
using Moonglade.Data.Spec;

namespace Moonglade.Core.TagFeature;

public record DeleteTagCommand(int Id) : IRequest<OperationCode>;

public class DeleteTagCommandHandler(
    MoongladeRepository<TagEntity> tagRepo,
    MoongladeRepository<PostTagEntity> postTagRepo)
    : IRequestHandler<DeleteTagCommand, OperationCode>
{
    public async Task<OperationCode> Handle(DeleteTagCommand request, CancellationToken ct)
    {
        var exists = await tagRepo.AnyAsync(c => c.Id == request.Id, ct);
        if (!exists) return OperationCode.ObjectNotFound;

        // 1. Delete Post-Tag Association
        var postTags = await postTagRepo.ListAsync(new PostTagByTagIdSpec(request.Id), ct);
        await postTagRepo.DeleteAsync(postTags, ct);

        // 2. Delte Tag itslef
        await tagRepo.DeleteAsync(request.Id, ct);

        return OperationCode.Done;
    }
}