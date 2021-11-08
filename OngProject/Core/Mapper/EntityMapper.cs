using System;
using System.Collections;
using System.Collections.Generic;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.SlidesDTOs;
using OngProject.Core.DTOs.UserDTOs;
using OngProject.Core.Entities;
using OngProject.Infrastructure.Repositories;

namespace OngProject.Core.Mapper
{
    public class EntityMapper
    {
        #region Slides Mappers
        public SlidesDTO FromSlideToSlideDto(Slides slide)
        {
            var slideDTO = new SlidesDTO()
            {
                ImageUrl = slide.ImageUrl,
                Order = slide.Order
            };
            return slideDTO;
        }
        public Slides FromSlidesDtoToSlides(int id, SlidesDTO slidesDTO)
        {
            var slide = new Slides()
            {
                Id= id,
                ImageUrl = slidesDTO.ImageUrl,
                Order = slidesDTO.Order,
                Text = slidesDTO.Text,
                OrganizationId = slidesDTO.OrganizationId
            };
            return slide;
        }
        public SlidesDTO FromSlideDetalleToSlideDto(Slides slide)
        {
            var slideDTO = new SlidesDTO()
            {
                ImageUrl = slide.ImageUrl,
                Order = slide.Order,
                Text = slide.Text,
                OrganizationId = slide.OrganizationId

            };
            return slideDTO;
        }

        public List<SlidesPublicDTO> FromSlidePublicToSlideDto(List<Slides> slides)
        {
            List<SlidesPublicDTO> publics = new List<SlidesPublicDTO>();
            foreach (var sd in slides)
            {
                var sPDTO = new SlidesPublicDTO()
                {
                    ImageUrl = sd.ImageUrl,
                    Order = sd.Order
                };
                publics.Add(sPDTO);
            }
            return publics;
        }

        #endregion

        #region Category Mappers
        public CategoryDTO FromCategoryToCategoryDto(Category category)
        {
            var categoryDTO = new CategoryDTO()
            {
                Name = category.Name,
                Description = category.Description,
                Image = category.Image
            };
            return categoryDTO;
        }

        public CategoryNameDTO FromCategoryToCategoryNameDto(Category category)
        {
            var categoryNameDTO = new CategoryNameDTO()
            {
                Name = category.Name
            };
            return categoryNameDTO;
        }

        public Category FromCategoryCreateDtoToCategory(CategoryDTO categoryDTO)
        {
            var category = new Category()
            {
                Name = categoryDTO.Name,
                Description = categoryDTO.Description,
                Image = categoryDTO.Image
            };
            return category;
        }

        #endregion

        #region News Mappers
        public NewsDTO FromNewsToNewsDTO(News news)
        {
            var newsDTO = new NewsDTO()
            {
                Name = news.Name,
                Content = news.Content,
                Image = news.Image,
                CategoryId = news.CategoryId
            };
            return newsDTO;
        }
        public News FromNewsDTOtoNews(NewsDTO newsDTO)
        {
            var news = new News()
            {
                Name = newsDTO.Name,
                Content = newsDTO.Content,
                Image = newsDTO.Image,
                CategoryId = newsDTO.CategoryId
            };
            return news;
        }
        public News NewsInsertDTOtoNews(NewsInsertDTO NewsInsertDTO)
        {
            var news = new News()
            {
                Name = NewsInsertDTO.Name,
                Content = NewsInsertDTO.Content,
                Image = NewsInsertDTO.Image.ToString(),
                CategoryId = NewsInsertDTO.CategoryId
            };
            return news;
        }

        #endregion

        #region Comments Mapper
        public CommentsDTO FromCommentsToCommentsDto(Comments comment)
        {
            var CommentsDTO = new CommentsDTO()
            {
                Body = comment.Body
            };
            return CommentsDTO;
        }
        public Comments FromNewCommentsDtoToComments(NewCommentsDTO newCommentDTO)
        {
            return new Comments()
            {
                Body = newCommentDTO.Body,
                NewId = newCommentDTO.NewId,
                UserId = newCommentDTO.UserId
            };
        }
        public Comments FromComentUpdateToComment (CommentUpdateDTO commentUpdateDTO, Comments comment)
        {
            if (commentUpdateDTO.UserId != null)
                comment.UserId = (int)commentUpdateDTO.UserId;

            if (commentUpdateDTO.NewId != null)
                comment.NewId = (int)commentUpdateDTO.NewId;

            if (commentUpdateDTO.Body != null)
                comment.Body = commentUpdateDTO.Body;

            return comment;
        }
        #endregion

        #region Member Mapper
        public MembersDTO FromMembersToMembersDto(Member member)
        {
            var membersDTO = new MembersDTO()
            {
                Name = member.Name,
                FacebookUrl = member.FacebookUrl,
                InstagramUrl = member.InstagramUrl,
                LinkedinUrl = member.LinkedinUrl,
                Image = member.Image,
                Description = member.Description

            };
            return membersDTO;
        }
        public Member FromMembersDTOtoMember(MemberInsertDTO membersDTO)
        {
            return new Member()
            { 
                Name = membersDTO.Name,
                FacebookUrl = membersDTO.FacebookUrl,
                InstagramUrl = membersDTO.InstagramUrl,
                LinkedinUrl = membersDTO.LinkedinUrl,
                Image = membersDTO.Image.ToString(),
                Description = membersDTO.Description
            };
        }

        #endregion

        #region Contact Mappers
        public ContactDTO FromContactsToContactsDto(Contacts contact)
        {
            var contactDTO = new ContactDTO()
            {
                Name = contact.Name,
                Phone = contact.Phone,
                Email = contact.Email,
                Message = contact.Message
            };
            return contactDTO;
        }
        public Contacts FromContactsDtoToContacts(ContactInsertDTO contactInsertDTO)
        {
            var contact = new Contacts()
            {
                Name = contactInsertDTO.Name,
                Phone = contactInsertDTO.Phone,
                Email = contactInsertDTO.Email,
                Message = contactInsertDTO.Message
            };
            return contact;
        }
        #endregion

        #region Organization Mappers
        public OrganizationsDTO FromOrganizationToOrganizationDto(Organizations organization)
        {
            return new OrganizationsDTO
            {
                Name = organization.Name,
                Image = organization.Image,
                Phone = organization.Phone,
                Address = organization.Address,
                FacebookUrl = organization.FacebookUrl,
                InstagramUrl = organization.InstagramUrl,
                LinkedinUrl = organization.LinkedinUrl
            };
        }
        #endregion

        #region User Mappers

        public UserDTO FromsUserToUserDto(User user)
        {
            var userDTO = new UserDTO()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                Photo = null,
                RoleId = user.RoleId
            };
            return userDTO;
        }
        public UserInfoDTO FromsUserToUserInfoDto(User user)
        {
            var userDTO = new UserInfoDTO()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Photo = user.Photo,
                RoleId = user.RoleId
            };
            return userDTO;
        }


        public User FromUserDtoToUser(UserInsertDTO userInsertDTO)
        {
            var user = new User()
            {
                FirstName = userInsertDTO.FirstName,
                LastName = userInsertDTO.LastName,
                Email = userInsertDTO.Email,
                Password = userInsertDTO.Password,
                RoleId = 2
            };
            if (userInsertDTO.Photo == null) user.Photo = null;
            else user.Photo = userInsertDTO.Photo.ToString();
            return user;
        }

        #endregion

        #region Activities Mappers
        public ActivitiesDTO FromActivitiesToActivitiesDTO(Activities activities)
        {
            var activitiesDTO = new ActivitiesDTO()
            {
                Name = activities.Name,
                Content = activities.Content,
                Image = activities.Image
            };
            return activitiesDTO;
        }
        public Activities FromActivitiesDTOToActivities(ActivitiesDTO activitiesDTO)
        {
            var activities = new Activities()
            {
                Name = activitiesDTO.Name,
                Content = activitiesDTO.Content,
                Image = activitiesDTO.Image
            };
            return activities;
        }

        public Activities ActivitiyInsertDTOtoActivity(ActivitiyInsertDTO activitiyInsertDTO)
        {
            var activity = new Activities()
            {
                Name = activitiyInsertDTO.Name,
                Content = activitiyInsertDTO.Content,
                Image = activitiyInsertDTO.Image.ToString()
            };
            return activity;
        }

        #endregion

        #region Testimonials Mappers
        public TestimonialsDTO FromTestimonialsToTestimonialsDTO (Testimonials testimonials)
        {
            return new TestimonialsDTO()
            {
                Content = testimonials.Content,
                Name = testimonials.Name,
                Imagen = testimonials.Image
            };
        }

        public Testimonials TestimonialsCreateDTOTestimonials(TestimonialsCreateDTO testimonialsCreateDTO)
        {
            var testimonial = new Testimonials()
            {
                Name = testimonialsCreateDTO.Name,
                Content = testimonialsCreateDTO.Content
            };

            return testimonial;
        }
        #endregion
    }
}
